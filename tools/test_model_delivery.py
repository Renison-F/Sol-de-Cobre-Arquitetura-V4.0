import copy, json, tempfile, unittest
from pathlib import Path
from validate_model_delivery import validate, ROOT, REVIEWS

class ModelDeliveryTests(unittest.TestCase):
    def setUp(self):
        self.temp=tempfile.TemporaryDirectory();self.folder=Path(self.temp.name)
        self.spec=json.loads((ROOT/'11-contratos/modelagem.json').read_text())['assets'][0]
        # Synthetic bytes exercise only the manifest/file contract, never FBX parsing.
        for path in ('Source/model.blend','Runtime/model.fbx','Textures/base.png','Evidence/device.txt'):
            f=self.folder/path;f.parent.mkdir(parents=True,exist_ok=True);f.write_bytes(b'SYNTHETIC TEST FIXTURE')
        self.data={'schema_version':1,'asset_id':self.spec['asset_id'],'rig_type':self.spec['rig_type'],
            'source_file':'Source/model.blend','runtime_file':'Runtime/model.fbx',
            'lods':[{'level':i,'triangles':n,'material_slots':2} for i,n in enumerate(self.spec['lod_triangles'])],
            'deform_bones':60,'max_influences':4,'textures':[{'file':'Textures/base.png','width':1024,'height':1024}],
            'sockets':self.spec['sockets'][:],'clips':self.spec['clips'][:],
            'metrics_source':'manual_verified','reviews':{k:True for k in REVIEWS},'evidence_files':['Evidence/device.txt']}
    def tearDown(self):self.temp.cleanup()
    def errors(self,accepted=False):return validate(self.data,self.spec,self.folder,accepted)
    def test_complete_manifest(self):self.assertEqual([],self.errors(True))
    def test_cannot_escape_package(self):
        self.data['runtime_file']='../outside.fbx';self.assertTrue(self.errors())
    def test_symlink_cannot_escape(self):
        with tempfile.TemporaryDirectory() as other:
            f=Path(other)/'outside.fbx';f.write_bytes(b'outside')
            (self.folder/'Runtime/link.fbx').symlink_to(f)
            self.data['runtime_file']='Runtime/link.fbx';self.assertTrue(self.errors())
    def test_over_budget_lod(self):
        self.data['lods'][1]['triangles']+=1;self.assertTrue(self.errors())
    def test_duplicate_lod(self):
        self.data['lods'][2]['level']=1;self.assertTrue(self.errors())
    def test_boolean_is_not_geometry_measurement(self):
        self.data['lods'][0]['triangles']=True;self.assertTrue(self.errors())
    def test_missing_required_clip(self):
        self.data['clips'].pop();self.assertTrue(self.errors())
    def test_more_skin_weights_are_rejected(self):
        self.data['max_influences']=8;self.assertTrue(self.errors())
    def test_source_file_is_required(self):
        (self.folder/'Source/model.blend').unlink();self.assertTrue(self.errors())
    def test_acceptance_needs_reviews_and_evidence(self):
        self.data['reviews']['device_profile']=False;self.data['evidence_files']=[]
        self.assertGreaterEqual(len(self.errors(True)),2)
    def test_oversized_texture(self):
        self.data['textures'][0]['width']=8192;self.assertTrue(self.errors())
    def test_wrong_rig_and_empty_socket(self):
        self.data['rig_type']='Generic';self.data['sockets']=[''];self.assertTrue(self.errors())
if __name__=='__main__':unittest.main()
