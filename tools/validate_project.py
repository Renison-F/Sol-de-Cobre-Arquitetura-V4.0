#!/usr/bin/env python3
"""Validate documentation/data, not Unity scenes or store integrations."""
from pathlib import Path
import json,re,csv
ROOT=Path(__file__).resolve().parents[1]
errors=[]
def check(value,msg):
    if not value:errors.append(msg)
def read(name):return json.loads((ROOT/name).read_text())
levels=read('11-contratos/campanha.json')['levels'];cast=read('11-contratos/elenco.json')['characters']
enemies=read('11-contratos/bestiario.json')['enemies'];tasks=read('11-contratos/backlog.json')['tasks']
check(len(levels)==10,'level count');check(sum(len(x['beats']) for x in levels)==60,'sequence count')
check(len(cast)==16,'cast count');check(len(enemies)==19,'enemy count')
for data in [cast,enemies,tasks]:
 ids=[x['id'] for x in data];check(len(ids)==len(set(ids)),'duplicate ID')
for x in cast+enemies:check((ROOT/x['art']).is_file(),'missing art '+x['id'])
for x in cast:check((ROOT/'02-personagens'/(x['slug']+'.md')).is_file(),'missing profile '+x['id'])
for x in levels:
 n=x['n'];code=f'{n:02}';check((ROOT/f"03-cenarios/{code}-{x['slug']}.md").is_file(),'missing level '+code)
 for path in [f'04-roteiro/episodios/EP-{code}.md',f'05-game-design/bosses/BOSS-{code}.md',f'05-missoes/principais/M{code}.md']:
  check((ROOT/path).is_file(),'missing content '+path)
 flags=[b[4] for b in x['beats']];check(len(flags)==len(set(flags)),'duplicate flag '+code)
check(len(list((ROOT/'09-assets/personagens').glob('*.png')))==22,'art count')
taskmap={x['id']:x for x in tasks};visited=set();active=set()
def walk(id):
 if id in active:errors.append('dependency cycle '+id);return
 if id in visited:return
 active.add(id)
 for dep in taskmap[id]['depends']:
  if dep not in taskmap:errors.append('unknown dependency '+dep)
  else:walk(dep)
 active.remove(id);visited.add(id)
for id in taskmap:walk(id)
check(sum(x['hours'] for x in tasks)==read('11-contratos/backlog.json')['base_hours'],'hours total')
with (ROOT/'11-contratos/dialogos-pt-BR.csv').open() as f:
 lines=list(csv.DictReader(f));ids=[x['id'] for x in lines];check(len(ids)==len(set(ids)),'duplicate dialogue ID')
for file in ROOT.rglob('*.md'):
 text=file.read_text()
 for path in re.findall(r'`((?:00-governanca|01-universo|02-personagens|03-cenarios|04-roteiro|05-game-design|05-missoes|06-arquitetura|07-producao|08-qualidade|09-arte-audio|09-assets|10-seguranca|11-contratos|12-desenvolvimento)/[^`]+\.(?:md|json|yaml|png|csv|sql))`',text):
  check((ROOT/path).is_file(),f'{file.relative_to(ROOT)} -> missing {path}')
if errors:print('\n'.join(errors));raise SystemExit(1)
print(f'OK: {len(levels)} levels / 60 sequences / {len(cast)} identities / {len(enemies)} enemies / 22 art sheets / {len(tasks)} tasks / {len(lines)} dialogue lines')
