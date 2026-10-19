-- PostgreSQL target schema; not executed against a live database in this package.
BEGIN;
CREATE TABLE installations (
 id text PRIMARY KEY, platform text NOT NULL CHECK(platform IN ('android','ios')),
 app_id text NOT NULL, key_thumbprint text NOT NULL, public_key bytea NOT NULL,
 attest_counter bigint NOT NULL DEFAULT 0 CHECK(attest_counter>=0),
 trust_level text NOT NULL CHECK(trust_level IN ('standard','reduced')),
 created_at timestamptz NOT NULL DEFAULT now(), last_seen_at timestamptz NOT NULL DEFAULT now(),
 UNIQUE(platform,app_id,key_thumbprint)
);
CREATE TABLE license_grants (
 jti text PRIMARY KEY, installation_id text NOT NULL REFERENCES installations(id),
 kid text NOT NULL, status text NOT NULL CHECK(status IN ('pending','issued','revoked','failed')),
 payload bytea NOT NULL, signed_envelope text,
 issued_at timestamptz NOT NULL, expires_at timestamptz NOT NULL,
 offline_until timestamptz NOT NULL,
 CHECK(expires_at>issued_at AND offline_until>=expires_at AND offline_until<=issued_at+interval '37 days'),
 CHECK(status<>'issued' OR signed_envelope IS NOT NULL)
);
CREATE INDEX license_installation ON license_grants(installation_id,issued_at DESC);
CREATE TABLE idempotency_requests (
 principal_id text NOT NULL REFERENCES installations(id), request_key text NOT NULL,
 body_sha256 bytea NOT NULL, state text NOT NULL CHECK(state IN ('pending','done','failed')),
 response_json jsonb, expires_at timestamptz NOT NULL,
 PRIMARY KEY(principal_id,request_key)
);
CREATE TABLE provider_events (
 provider text NOT NULL, event_id text NOT NULL, verified_at timestamptz NOT NULL,
 sanitized_kind text NOT NULL, applied_at timestamptz,
 PRIMARY KEY(provider,event_id)
);
CREATE TABLE audit_events (
 id bigint GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
 happened_at timestamptz NOT NULL DEFAULT now(), actor_ref text NOT NULL,
 event_kind text NOT NULL, correlation_id text NOT NULL,
 metadata jsonb NOT NULL DEFAULT '{}'::jsonb
);
-- Production policies: application role has least privilege, no public network DB,
-- sanitized audit metadata only; operational retention jobs and encrypted backups.
COMMIT;
