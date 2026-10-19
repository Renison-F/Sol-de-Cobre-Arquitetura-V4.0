#!/usr/bin/env python3
"""Production-spec consistency and assembly graph; does not compile Unity."""
from pathlib import Path
import csv,json,re,sys
ROOT=Path(__file__).resolve().parents[1]
errors=[]
def check(value,message):
    if not value:errors.append(message)
spec=json.loads((ROOT/'11-contratos/modelagem.json').read_text())['assets']
cast=json.loads((ROOT/'11-contratos/elenco.json').read_text())['characters']
check(len(spec)==17,'Expected 17 individual production specs')
check({x['asset_id'] for x in spec}=={x['id'] for x in cast}|{'PROP-MAQUINA-SECA'},'Production identities differ from cast')
check(len({x['slug'] for x in spec})==17,'Duplicate slug')
for x in spec:
    for field in ('concept','profile'):check((ROOT/x[field]).is_file(),'Missing '+x[field])
    check(x['rig_type'] in ('Humanoid','Generic'),'Invalid rig '+x['slug'])
    check(x['lod_triangles'][0]>x['lod_triangles'][1]>x['lod_triangles'][2]>0,'LOD targets '+x['slug'])
    check(x['max_influences']==4,'Skin influence baseline '+x['slug'])
    for field in ('sockets','clips'):check(bool(x[field]) and len(x[field])==len(set(x[field])),'Invalid '+field+' '+x['slug'])
    c=next((c for c in cast if c['id']==x['asset_id']),None)
    if c:check(set(c['animations'])<=set(x['clips']),'Animation contract lost '+x['slug'])
secondary=json.loads((ROOT/'11-contratos/modelagem-secundarios.json').read_text())['assets']
enemies=json.loads((ROOT/'11-contratos/bestiario.json').read_text())['enemies']
check(len(secondary)==22,'Expected 19 enemies and 3 NPC family targets')
check({x['asset_id'] for x in secondary}=={x['id'] for x in enemies}|{'NPC-01','NPC-02','NPC-03'},'Secondary identity coverage')
for x in secondary:
    for field in ('concept','profile'):check((ROOT/x[field]).is_file(),'Missing '+x[field])
    check(x['lod_triangles'][0]>x['lod_triangles'][1]>x['lod_triangles'][2]>0,'Secondary LOD targets '+x['asset_id'])
rows=list(csv.DictReader((ROOT/'11-contratos/modelagem-resumo.csv').open()))
check(len(rows)==len(spec),'CSV row count')
by_id={r['asset_id']:r for r in rows}
for x in spec:
    row=by_id.get(x['asset_id'],{})
    for i in range(3):check(row.get(f'lod{i}_tri')==str(x['lod_triangles'][i]),'CSV LOD mismatch '+x['slug'])
    for key,target in [('name','name'),('rig','rig_type'),('bones_max','max_deform_bones'),('materials_max','max_material_slots'),('texture_edge_max','max_texture_edge')]:
        check(row.get(key)==str(x[target]),'CSV mismatch '+x['slug']+' '+key)
game=ROOT/'unity-project-template/Assets/_Game'
assemblies={}
for f in game.rglob('*.asmdef'):
    obj=json.loads(f.read_text());check(obj['name'] not in assemblies,'Duplicate assembly');assemblies[obj['name']]=obj
check(len(assemblies)==4,'Expected Domain, Runtime, Editor and EditMode assemblies')
check(assemblies['SolDeCobre.Domain']['noEngineReferences'] is True,'Domain must not reference UnityEngine')
for f in (game/'Domain').rglob('*.cs'):
    check(not re.search(r'\b(?:using\s+Unity|UnityEngine\.|QuestDefinition)',f.read_text()),'Domain dependency '+str(f))
allowed_external={'Unity.InputSystem'}
active=set();done=set()
def visit(name):
    if name in active:errors.append('Assembly dependency cycle '+name);return
    if name in done:return
    active.add(name)
    for ref in assemblies[name]['references']:
        if ref in assemblies:visit(ref)
        else:check(ref in allowed_external,'Unknown assembly reference '+ref)
    active.remove(name);done.add(name)
for name in assemblies:visit(name)
check(assemblies['SolDeCobre.Editor']['includePlatforms']==['Editor'],'Editor assembly platform')
check(assemblies['SolDeCobre.Tests.EditMode']['includePlatforms']==['Editor'],'Tests assembly platform')
if errors:print('\n'.join(errors));sys.exit(1)
print('OK: 17 main + 22 secondary targets, cast/clip references, CSV budgets and 4 acyclic assemblies. No Unity compilation claimed.')
