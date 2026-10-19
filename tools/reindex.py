#!/usr/bin/env python3
"""Atualiza o snapshot offline quando arquivos Markdown/código mudarem no disco.
Execute: python tools/reindex.py a partir do diretório da raiz do pacote.
"""
from pathlib import Path
import json,os,re
ROOT=Path(__file__).resolve().parent.parent
TARGET=ROOT/'dashboard'/'data'/'catalog.js'
TEXT_EXT={'.md','.cs','.json','.js','.css','.html','.yml','.yaml','.py','.txt','.xml','.asmdef','.shader','.svg','.sql','.csv'}
IMAGE_EXT={'.png','.jpg','.jpeg','.webp'}
SKIP={'__pycache__','.git','node_modules','.idea','.vscode'}
docs=[];files=[]
for folder,dirs,names in os.walk(ROOT):
    dirs[:]=sorted(d for d in dirs if d not in SKIP and not d.startswith('.') and not (Path(folder)/d).is_symlink())
    for name in sorted(names):
        f=Path(folder)/name;path=f.relative_to(ROOT).as_posix();suffix=f.suffix.lower()
        if f.is_symlink() or f==TARGET:continue
        if suffix not in TEXT_EXT|IMAGE_EXT:continue
        if suffix in IMAGE_EXT:files.append({'path':path,'type':'image','size':f.stat().st_size});continue
        if f.stat().st_size>1500000:continue
        content=f.read_text(encoding='utf-8',errors='replace');files.append({'path':path,'type':'text','content':content,'size':f.stat().st_size})
        if suffix=='.md':
            m=re.search(r'^#\s+(.+)$',content,re.M)
            docs.append({'path':path,'title':m.group(1).strip() if m else f.stem,'content':content})
files.sort(key=lambda x:x['path']);docs.sort(key=lambda x:x['path']);payload={'docs':docs,'files':files,'meta':{'docs':len(docs),'files':len(files),'mode':'indexed-offline','version':'4.0'}}
TARGET.parent.mkdir(parents=True,exist_ok=True)
TARGET.write_text('/* Gerado por python tools/reindex.py — snapshot de leitura para file:// */\nwindow.SOL_COBRE='+json.dumps(payload,ensure_ascii=False,separators=(',',':'))+';\n',encoding='utf-8')
print('Catálogo atualizado:',len(docs),'Markdown,',len(files),'arquivos. Tamanho:',TARGET.stat().st_size)
