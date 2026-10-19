#!/usr/bin/env python3
"""Validate a 3D handoff manifest and files, not FBX geometry or artistic quality."""
from pathlib import Path
import argparse, json, sys
ROOT = Path(__file__).resolve().parents[1]
REVIEWS = ('technical_views', 'deformation', 'unity_import', 'lod_transition', 'device_profile', 'origin_recorded')

def validate(data, spec, folder, require_accepted=False):
    errors=[]
    def check(value, message):
        if not value: errors.append(message)
    def integer(value, minimum=0):
        return type(value) is int and value >= minimum
    def file_at(value, suffixes=None):
        if not isinstance(value,str) or not value or '\\' in value:
            errors.append('File path must be a nonempty relative POSIX path'); return
        rel=Path(value)
        if rel.is_absolute() or '..' in rel.parts or ':' in value:
            errors.append('File outside package: '+value); return
        f=(folder/rel).resolve()
        if not f.is_relative_to(folder.resolve()):
            errors.append('File outside package: '+value); return
        if not f.is_file() or f.stat().st_size==0:
            errors.append('Missing/empty file: '+value)
        if suffixes and f.suffix.lower() not in suffixes:
            errors.append('Unexpected format: '+value)
    if not isinstance(data,dict): return ['Manifest must be an object']
    check(type(data.get('schema_version')) is int and data['schema_version']==1,'Unsupported manifest schema')
    check(data.get('asset_id')==spec['asset_id'],'Wrong asset_id')
    check(data.get('rig_type')==spec['rig_type'],'Wrong rig_type')
    file_at(data.get('source_file'),{'.blend'})
    file_at(data.get('runtime_file'),{'.fbx'})
    lods=data.get('lods')
    if not isinstance(lods,list) or len(lods)!=3:
        errors.append('Provide exactly LOD0, LOD1 and LOD2');lods=[]
    seen=set()
    for lod in lods:
        if not isinstance(lod,dict):errors.append('LOD must be an object');continue
        level=lod.get('level')
        if not integer(level) or level not in (0,1,2) or level in seen:
            errors.append('Invalid/duplicate LOD level');continue
        seen.add(level)
        tri=lod.get('triangles');slots=lod.get('material_slots')
        check(integer(tri,1) and tri<=spec['lod_triangles'][level],f'LOD{level} triangle budget/measurement invalid')
        check(integer(slots,1) and slots<=spec['max_material_slots'],f'LOD{level} material slots invalid')
    check(seen=={0,1,2},'LOD levels incomplete')
    for field,limit in [('deform_bones',spec['max_deform_bones']),('max_influences',spec['max_influences'])]:
        value=data.get(field);check(integer(value) and value<=limit,field+' invalid/over budget')
    textures=data.get('textures')
    if not isinstance(textures,list) or not textures:
        errors.append('Provide texture paths and measured dimensions');textures=[]
    for tex in textures:
        if not isinstance(tex,dict):errors.append('Invalid texture record');continue
        file_at(tex.get('file'),{'.png','.tga','.exr'})
        for field in ('width','height'):
            v=tex.get(field);check(integer(v,1) and v<=spec['max_texture_edge'],'Texture '+field+' invalid/over budget')
    for field in ('sockets','clips'):
        values=data.get(field)
        if not isinstance(values,list) or not all(isinstance(v,str) and v for v in values):
            errors.append(field+' must contain names');continue
        check(len(values)==len(set(values)),field+' contains duplicates')
        check(set(spec[field])<=set(values),field+' missing: '+', '.join(sorted(set(spec[field])-set(values))))
    check(data.get('metrics_source') in ('blender_and_unity','manual_verified'),'Record how metrics were measured')
    reviews=data.get('reviews')
    if not isinstance(reviews,dict):reviews={}
    if require_accepted:
        for key in REVIEWS:check(reviews.get(key) is True,'Review missing: '+key)
        evidence=data.get('evidence_files')
        if not isinstance(evidence,list) or not evidence:errors.append('Accepted delivery needs evidence files')
        else:
            for f in evidence:file_at(f)
    return errors

def main():
    parser=argparse.ArgumentParser(description=__doc__)
    parser.add_argument('manifest',type=Path)
    parser.add_argument('--catalog',type=Path,default=ROOT/'11-contratos/modelagem.json')
    parser.add_argument('--require-accepted',action='store_true')
    args=parser.parse_args()
    try:
        data=json.loads(args.manifest.read_text(encoding='utf-8'))
        catalog=json.loads(args.catalog.read_text(encoding='utf-8'))
        asset_id=data.get('asset_id') if isinstance(data,dict) else None
        spec=next((x for x in catalog['assets'] if x['asset_id']==asset_id),None)
        errors=['Unknown asset_id'] if spec is None else validate(data,spec,args.manifest.parent,args.require_accepted)
    except (OSError,ValueError,KeyError,TypeError) as exc:
        print('Invalid delivery:',str(exc));return 1
    if errors:
        print('\n'.join(errors));return 1
    print('OK: manifest, file presence and declared budgets. Geometry, rig and visual quality require Blender/Unity review.')
    return 0
if __name__=='__main__':sys.exit(main())
