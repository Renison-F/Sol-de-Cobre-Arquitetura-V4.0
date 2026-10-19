#!/usr/bin/env python3
"""Local documentation reader. Never use as a public game/license server."""
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer
from urllib.parse import urlsplit, parse_qs, unquote
from pathlib import Path
import json, os, mimetypes
ROOT=Path(__file__).resolve().parent
TEXT={'.md','.markdown','.cs','.json','.js','.css','.html','.yml','.yaml','.py','.txt','.xml','.asmdef','.shader','.svg','.sql','.csv'}
IMAGES={'.png','.jpg','.jpeg','.webp'}
STATIC=TEXT|IMAGES|{'.woff2','.woff','.ico'}
DENY={'__pycache__','.git','.env','node_modules','Library','Temp','Obj','Logs','.idea','.vscode'}
MAX_READ=1_500_000

def resolve_safe(value,static=False):
    if not value or value.startswith(('/', '\\')) or '\\' in value or '\x00' in value:
        raise PermissionError('path')
    parts=Path(value).parts
    if any(x in DENY or x.startswith('.') or x=='..' for x in parts): raise PermissionError('path')
    path=ROOT
    for part in parts:
        path=path/part
        if path.is_symlink():raise PermissionError('symlink')
    resolved=path.resolve()
    try:resolved.relative_to(ROOT)
    except ValueError:raise PermissionError('outside')
    if resolved.suffix.lower() not in (STATIC if static else TEXT|IMAGES):raise PermissionError('extension')
    return resolved

def list_files():
    items=[]
    for folder,dirs,files in os.walk(ROOT):
        dirs[:]=sorted(d for d in dirs if d not in DENY and not d.startswith('.') and not (Path(folder)/d).is_symlink())
        for name in sorted(files):
            raw=Path(folder,name);rel=raw.relative_to(ROOT).as_posix()
            try:path=resolve_safe(rel)
            except PermissionError:continue
            if path.is_file():items.append({'path':rel,'size':path.stat().st_size,'type':'image' if path.suffix.lower() in IMAGES else 'text'})
    return items

class Handler(BaseHTTPRequestHandler):
    server_version='SolCobreDocs/4.0'
    def end_headers(self):
        self.send_header('X-Content-Type-Options','nosniff')
        self.send_header('Referrer-Policy','no-referrer')
        self.send_header('Content-Security-Policy',"default-src 'self'; img-src 'self' data:; style-src 'self' 'unsafe-inline'; script-src 'self'; font-src 'self'; connect-src 'self'; frame-ancestors 'none'; object-src 'none'; base-uri 'none'; form-action 'none'")
        super().end_headers()
    def _json(self,obj,status=200,head=False):
        out=json.dumps(obj,ensure_ascii=False).encode('utf-8')
        self.send_response(status);self.send_header('Content-Type','application/json; charset=utf-8')
        self.send_header('Cache-Control','no-store');self.send_header('Content-Length',str(len(out)));self.end_headers()
        if not head:self.wfile.write(out)
    def _local_request(self):
        port=self.server.server_address[1]
        hosts={f'127.0.0.1:{port}',f'localhost:{port}'}
        if self.headers.get('Host','') not in hosts:return False
        origin=self.headers.get('Origin')
        return origin is None or origin in {f'http://{host}' for host in hosts}
    def _read(self,head=False):
        if not self._local_request():return self._json({'error':'Host/origem não permitidos'},403,head)
        parsed=urlsplit(self.path)
        if parsed.path=='/api/health':return self._json({'service':'sol-cobre-docs','readonly':True,'version':'4.0'},head=head)
        if parsed.path=='/api/tree':return self._json({'files':list_files()},head=head)
        if parsed.path=='/api/file':
            value=parse_qs(parsed.query).get('path',[''])[0]
            try:path=resolve_safe(value)
            except PermissionError:return self._json({'error':'Caminho não permitido'},403,head)
            if not path.is_file():return self._json({'error':'Não encontrado'},404,head)
            if path.stat().st_size>MAX_READ:return self._json({'error':'Prévia excede 1,5 MB'},413,head)
            if path.suffix.lower() in IMAGES:return self._json({'error':'Use URL da imagem'},415,head)
            try:content=path.read_text(encoding='utf-8')
            except UnicodeError:return self._json({'error':'Não é texto UTF-8'},415,head)
            return self._json({'path':value,'content':content,'size':path.stat().st_size},head=head)
        raw=unquote(parsed.path).lstrip('/') or 'index.html'
        try:path=resolve_safe(raw,static=True)
        except PermissionError:return self._json({'error':'Caminho não permitido'},403,head)
        if not path.is_file():return self._json({'error':'Não encontrado'},404,head)
        mime=mimetypes.guess_type(path.name)[0] or 'application/octet-stream'
        if path.suffix in {'.md','.cs','.py','.sql','.yml','.yaml','.txt'}:mime='text/plain'
        if mime.startswith('text/') or path.suffix in {'.js','.json'}:mime+='; charset=utf-8'
        self.send_response(200);self.send_header('Content-Type',mime)
        self.send_header('Content-Length',str(path.stat().st_size));self.send_header('Cache-Control','no-cache');self.end_headers()
        if not head:
            with path.open('rb') as stream:
                while chunk:=stream.read(65536):self.wfile.write(chunk)
    def do_GET(self):self._read()
    def do_HEAD(self):self._read(head=True)
    def do_POST(self):self._json({'error':'Somente leitura'},405)
    def do_PUT(self):self._json({'error':'Somente leitura'},405)
    def do_DELETE(self):self._json({'error':'Somente leitura'},405)
    def do_PATCH(self):self._json({'error':'Somente leitura'},405)

if __name__=='__main__':
    import argparse
    parser=argparse.ArgumentParser(description='Documentação local — não é backend do jogo')
    parser.add_argument('--port',type=int,default=8765);args=parser.parse_args()
    print(f'Abra http://127.0.0.1:{args.port}/ (Ctrl+C para encerrar)',flush=True)
    ThreadingHTTPServer(('127.0.0.1',args.port),Handler).serve_forever()
