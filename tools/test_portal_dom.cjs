const {JSDOM,VirtualConsole}=require('./qa/node_modules/jsdom');
const fs=require('fs');const path=require('path');
const root=path.resolve(__dirname,'..');
let server;process.on('exit',()=>server?.kill());
const delay=ms=>new Promise(r=>setTimeout(r,ms));
(async()=>{
 server=require('child_process').spawn('python',[path.join(root,'serve.py')],{cwd:root,stdio:['ignore','pipe','ignore']});
 await new Promise((resolve,reject)=>{server.stdout.once('data',resolve);server.once('error',reject)});
 const errors=[];
 const vc=new VirtualConsole();vc.on('jsdomError',e=>{if(!e.message.includes('CSS'))errors.push(e.message)});
 function setup(w){w.scrollTo=()=>{};w.matchMedia=q=>({matches:false,addEventListener(){},removeEventListener(){}});w.fetch=(u,o)=>fetch(new URL(u,w.location.href),o);w.AbortController=AbortController;}
 const dom=await JSDOM.fromURL('http://127.0.0.1:8765/',{resources:'usable',runScripts:'dangerously',pretendToBeVisual:true,virtualConsole:vc,beforeParse:setup});
 const w=dom.window;await new Promise(r=>w.addEventListener('load',r));await delay(100);
 const doc=w.document;const nav=[...doc.querySelectorAll('#mainNav .nav-item')].map(n=>n.dataset.id);
 if(nav.length<20)throw Error('Navigation not initialized');
 for(const id of nav){
  doc.querySelector('#mainNav [data-id="'+id+'"]').click();
  if(doc.querySelector('#appMain').textContent.length<30)throw Error('Empty '+id);
  for(const el of doc.querySelectorAll('#appMain [data-path]')){
   const p=el.dataset.path;if(p && !fs.existsSync(path.join(root,p)))throw Error('Missing target '+id+': '+p);
  }
 }
 doc.querySelector('#mainNav [data-id="map"]').click();
 const tracks=[...doc.querySelectorAll('.campaign-track')];
 if(tracks.length!==2 || tracks[0].getAttribute('d')!==tracks[1].getAttribute('d'))throw Error('Map track layers disagree');
 const numbers=tracks[0].getAttribute('d').match(/-?\d+(?:\.\d+)?/g).map(Number);
 const endpoints=[[numbers[0],numbers[1]]];
 for(let i=2;i<numbers.length;i+=6)endpoints.push([numbers[i+4],numbers[i+5]]);
 const stops=[...doc.querySelectorAll('.map-node')];
 if(endpoints.length!==10 || stops.length!==10)throw Error('Map stop count');
 stops.forEach((stop,i)=>{
  const circle=stop.querySelector('circle');
  if(Math.hypot(+circle.getAttribute('cx')-endpoints[i][0],+circle.getAttribute('cy')-endpoints[i][1])>0.001)throw Error('Map phase off track '+(i+1));
  const label=[...stop.querySelectorAll('.campaign-stop-label tspan')].map(n=>n.textContent).join(' ');
  if(label!==stop.querySelector('title').textContent)throw Error('Truncated map label '+(i+1));
 });
 if(process.env.SOL_MAP_QA_EXPORT)fs.writeFileSync(process.env.SOL_MAP_QA_EXPORT,doc.querySelector('.campaign-map').outerHTML);
 doc.querySelector('#mainNav [data-id="gallery"]').click();
 if(doc.querySelectorAll('.gallery-item').length!==21)throw Error('Art count');
 const arts=[...doc.querySelectorAll('.gallery-item')];
 if(arts.some(n=>n.dataset.path.endsWith('/ze-e-trovico.png')))throw Error('Redundant duo gallery item');
 if(new Set(arts.map(n=>n.dataset.path)).size!==arts.length)throw Error('Duplicate gallery path');
 doc.querySelector('.gallery-item').click();if(doc.querySelector('#lightbox').getAttribute('aria-hidden')!=='false')throw Error('Lightbox');
 doc.querySelector('[data-action="close-lightbox"]').click();
 doc.querySelector('#mainNav [data-id="security"]').click();
 doc.querySelector('[data-path="10-seguranca/00-SEGURANCA.md"]').click();
 if(!doc.querySelector('#docArticle').textContent.includes('Fronteiras de confiança'))throw Error('Document markdown');
 doc.querySelector('[data-action="search"]').click();
 const search=doc.querySelector('#globalSearch');search.value='Isadora';search.dispatchEvent(new w.Event('input'));
 if(!doc.querySelector('#searchResults').textContent.includes('Isadora'))throw Error('Search');
 doc.querySelector('[data-action="close-search"]').click();
 doc.querySelector('#themeButton').click();if(doc.documentElement.dataset.theme!=='dark')throw Error('Theme');
 doc.querySelector('#mainNav [data-id="explorer"]').click();await delay(100);
 if(!doc.querySelector('.explorer-state').textContent.includes('AO VIVO'))throw Error('Live mode');
 const el=doc.querySelector('[data-action="explorer-file"][data-path="security-reference/licensing.py"]');if(!el)throw Error('No code path');el.click();await delay(100);
 if(!doc.querySelector('.line-code').textContent.includes('class Issuer'))throw Error('Live code read');
 // Menu logic at a mobile breakpoint; jsdom does not validate rendered layout.
 w.matchMedia=()=>({matches:true});doc.querySelector('[data-action="open-menu"]').click();
 if(!doc.querySelector('#sidebar').classList.contains('open'))throw Error('Menu');
 doc.querySelector('#mainNav [data-id="development"]').click();if(doc.querySelector('#sidebar').classList.contains('open'))throw Error('Menu close');
 dom.window.close();
 const offline=await JSDOM.fromFile(path.join(root,'index.html'),{resources:'usable',runScripts:'dangerously',pretendToBeVisual:true,virtualConsole:vc,beforeParse:setup});
 await new Promise(r=>offline.window.addEventListener('load',r));await delay(50);
 const od=offline.window.document;od.querySelector('#mainNav [data-id="gallery"]').click();
 if(od.querySelectorAll('.gallery-item').length!==21)throw Error('Offline gallery');
 od.querySelector('#mainNav [data-id="explorer"]').click();if(!od.querySelector('.explorer-state').textContent.includes('OFFLINE'))throw Error('Offline explorer');
 offline.window.close();
 if(errors.length)throw Error(errors.join('\n'));
 const result={mode:'jsdom DOM integration, no pixel rendering',views:nav,artSheets:21,search:true,theme:true,lightbox:true,liveFile:true,offline:true,mobileMenuLogic:true,mapStopsOnTrack:10,mapLayerPathsIdentical:true,mapLabelsComplete:true,errors};
 fs.writeFileSync(path.join(root,'08-qualidade/dom-results.json'),JSON.stringify(result,null,2));server.kill();console.log('PASS '+nav.length+' views; 21 art targets; markdown, search, theme, lightbox, live/offline, mobile menu logic');
})().catch(e=>{console.error(e);process.exit(1)});
