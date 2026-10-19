#!/usr/bin/env python3
"""Self-contained security/integration tests for the local documentation server."""
import importlib.util,json,threading,unittest,urllib.request,urllib.error
from pathlib import Path
from http.server import ThreadingHTTPServer
ROOT=Path(__file__).resolve().parents[1]
spec=importlib.util.spec_from_file_location('docs_serve',ROOT/'serve.py');module=importlib.util.module_from_spec(spec);spec.loader.exec_module(module)
class DocsServerTests(unittest.TestCase):
 @classmethod
 def setUpClass(cls):
  cls.server=ThreadingHTTPServer(('127.0.0.1',0),module.Handler)
  cls.thread=threading.Thread(target=cls.server.serve_forever,daemon=True);cls.thread.start()
  cls.base='http://127.0.0.1:'+str(cls.server.server_address[1])
 @classmethod
 def tearDownClass(cls):cls.server.shutdown();cls.server.server_close()
 def request(self,path,method='GET',headers=None):
  req=urllib.request.Request(self.base+path,method=method,headers=headers or {})
  try:
   with urllib.request.urlopen(req) as r:return r.status,r.read(),r.headers
  except urllib.error.HTTPError as r:return r.code,r.read(),r.headers
 def test_health(self):self.assertEqual(json.loads(self.request('/api/health')[1])['version'],'4.0')
 def test_api_traversal(self):self.assertEqual(self.request('/api/file?path=../../etc/passwd')[0],403)
 def test_static_traversal(self):self.assertEqual(self.request('/%2e%2e/etc/passwd')[0],403)
 def test_hidden_file(self):self.assertEqual(self.request('/api/file?path=.env')[0],403)
 def test_static_hidden(self):self.assertEqual(self.request('/.env')[0],403)
 def test_host(self):self.assertEqual(self.request('/api/health',headers={'Host':'attacker.invalid'})[0],403)
 def test_origin(self):self.assertEqual(self.request('/api/health',headers={'Origin':'https://attacker.invalid'})[0],403)
 def test_no_directory_listing(self):self.assertEqual(self.request('/10-seguranca/')[0],403)
 def test_no_write(self):
  for verb in ['POST','PUT','PATCH','DELETE']:
   with self.subTest(method=verb):self.assertEqual(self.request('/README.md',method=verb)[0],405)
 def test_static_and_head(self):
  status,body,headers=self.request('/README.md');self.assertEqual(status,200);self.assertTrue(body)
  status,body,headers=self.request('/README.md',method='HEAD');self.assertEqual(status,200);self.assertEqual(body,b'')
 def test_security_headers(self):
  _,_,headers=self.request('/');self.assertEqual(headers['X-Content-Type-Options'],'nosniff');self.assertIn("frame-ancestors 'none'",headers['Content-Security-Policy'])
 def test_symlink(self):
  link=ROOT/'qa-symlink-test.txt'
  try:
   link.symlink_to('/etc/passwd')
   self.assertEqual(self.request('/qa-symlink-test.txt')[0],403)
   self.assertEqual(self.request('/api/file?path=qa-symlink-test.txt')[0],403)
   self.assertNotIn('qa-symlink-test.txt',[i['path'] for i in json.loads(self.request('/api/tree')[1])['files']])
  finally:link.unlink(missing_ok=True)
 def test_live_refresh(self):
  file=ROOT/'tools/qa-live-test.md'
  try:
   file.write_text('# First\n');paths=[i['path'] for i in json.loads(self.request('/api/tree')[1])['files']]
   self.assertIn('tools/qa-live-test.md',paths)
   file.write_text('# Second\n');self.assertEqual(json.loads(self.request('/api/file?path=tools/qa-live-test.md')[1])['content'],'# Second\n')
  finally:file.unlink(missing_ok=True)
 def test_large_art_static(self):self.assertEqual(self.request('/09-assets/personagens/barao-ferrugem.png',method='HEAD')[0],200)
 def test_sql_preview(self):self.assertIn('CREATE TABLE',json.loads(self.request('/api/file?path=11-contratos/licensing.sql')[1])['content'])
if __name__=='__main__':unittest.main()
