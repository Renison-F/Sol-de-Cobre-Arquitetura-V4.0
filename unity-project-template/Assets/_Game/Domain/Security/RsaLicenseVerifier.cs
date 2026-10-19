using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
namespace SolDeCobre.Security
{
    // SC1 format: fixed algorithm, exact canonical ASCII payload, public keys only.
    // Compile/device-test RSA provider with the chosen Unity IL2CPP backend before shipping.
    public sealed class RsaLicenseVerifier : ILicenseVerifier
    {
        const string Safe = "[A-Za-z0-9_.:-]{1,128}";
        const string Number = "(?:0|[1-9][0-9]{0,18})";
        static readonly Regex Claims = new Regex(
            "\\A\\{\"app\":\"(?<app>"+Safe+")\",\"exp\":(?<exp>"+Number+
            "),\"iat\":(?<iat>"+Number+"),\"installation\":\"(?<installation>"+Safe+
            ")\",\"jti\":\"(?<jti>"+Safe+")\",\"offline_until\":(?<until>"+Number+
            "),\"platform\":\"(?<platform>"+Safe+")\",\"subject\":\"(?<subject>"+Safe+
            ")\",\"v\":1\\}\\z", RegexOptions.CultureInvariant);
        readonly IReadOnlyDictionary<string,RSAParameters> keys;
        readonly string app, platform;
        readonly Func<long> clock;
        readonly Func<string,bool> revoked;
        public RsaLicenseVerifier(IReadOnlyDictionary<string,RSAParameters> keys,
            string app, string platform, Func<long> clock, Func<string,bool> revoked)
        {
            this.keys=keys ?? throw new ArgumentNullException(nameof(keys));
            this.app=app; this.platform=platform;
            this.clock=clock ?? throw new ArgumentNullException(nameof(clock));
            this.revoked=revoked ?? throw new ArgumentNullException(nameof(revoked));
        }
        public LicenseWindow Verify(string token, string expectedInstallation)
        {
            if(string.IsNullOrEmpty(token) || token.Length>8192) throw new FormatException("token");
            var part=token.Split('.');
            if(part.Length!=4 || part[0]!="sc1" || !keys.TryGetValue(part[1],out var key))
                throw new CryptographicException("key");
            if(key.Modulus==null || key.Modulus.Length<384) throw new CryptographicException("key size");
            using(var rsa=RSA.Create())
            {
                rsa.ImportParameters(key);
                if(!rsa.VerifyData(Encoding.ASCII.GetBytes(string.Join(".",part,0,3)),
                    Decode(part[3]),HashAlgorithmName.SHA256,RSASignaturePadding.Pkcs1))
                    throw new CryptographicException("signature");
            }
            var payload=Decode(part[2]);
            foreach(byte value in payload) if(value>127) throw new FormatException("ASCII claims");
            var match=Claims.Match(Encoding.ASCII.GetString(payload));
            if(!match.Success || match.Groups["app"].Value!=app ||
                match.Groups["platform"].Value!=platform || match.Groups["installation"].Value!=expectedInstallation)
                throw new CryptographicException("binding");
            if(revoked(match.Groups["jti"].Value)) throw new CryptographicException("revoked");
            long iat=Read(match,"iat"),exp=Read(match,"exp"),until=Read(match,"until");
            long now=clock();
            if(iat>now+300 || exp<=iat || until<exp || until-iat>37L*86400L || now>=until)
                throw new CryptographicException("time");
            return new LicenseWindow(iat,exp,until);
        }
        static long Read(Match m,string key)
        {
            if(!long.TryParse(m.Groups[key].Value,NumberStyles.None,CultureInfo.InvariantCulture,out long result))
                throw new FormatException("number");
            return result;
        }
        static byte[] Decode(string value)
        {
            if(!Regex.IsMatch(value,"\\A[A-Za-z0-9_-]+\\z",RegexOptions.CultureInvariant))
                throw new FormatException("base64url");
            var raw=Convert.FromBase64String(value.Replace('-','+').Replace('_','/')+new string('=',(4-value.Length%4)%4));
            string canonical=Convert.ToBase64String(raw).TrimEnd('=').Replace('+','-').Replace('/','_');
            if(canonical!=value) throw new FormatException("base64url");
            return raw;
        }
    }
}
