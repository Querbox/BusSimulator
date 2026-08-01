const mini = document.querySelector('#map');
const mctx = mini.getContext('2d');
const $ = id => document.getElementById(id);

// Coordinates shared with the Unity route data. The web game now drives through
// the real Hechingen/Boll map instead of an invented canvas landscape.
const stops = [
  {name:'Hechingen Bahnhof',lat:48.3767,lon:8.7544},
  {name:'Hechingen Marktplatz',lat:48.3780,lon:8.7558},
  {name:'Hechingen Schulzentrum',lat:48.3725,lon:8.7620},
  {name:'Hechingen Am Hang',lat:48.3650,lon:8.7500},
  {name:'Hechingen Zum Biegel',lat:48.3695,lon:8.7450},
  {name:'Boll Ortsmitte',lat:48.3850,lon:8.7750},
  {name:'Boll Kirche',lat:48.3865,lon:8.7760},
  {name:'Burg Parkplatz',lat:48.3355,lon:8.8030},
  {name:'Burg Hohenzollern',lat:48.3333,lon:8.8047}
];
const state={lat:stops[0].lat,lon:stops[0].lon,angle:42,speed:0,keys:{},started:false,paused:false,doors:false,stop:1,passengers:12,score:92,time:7*60+42,cam:0};
const routeGeo={type:'Feature',geometry:{type:'LineString',coordinates:stops.map(s=>[s.lon,s.lat])}};
let map;
let busMarker;

function createBus(){
  const bus=document.createElement('div');
  bus.className='bus-3d';
  bus.innerHTML='<div class="bus-body"></div>';
  return bus;
}

function initMap(){
  if(!window.maplibregl){
    $('mapError').textContent='Die Online-Karte konnte nicht geladen werden.';
    return;
  }
  map=new maplibregl.Map({
    container:'game',
    style:'https://tiles.openfreemap.org/styles/liberty',
    center:[state.lon,state.lat],zoom:16.7,pitch:62,bearing:state.angle-180,
    attributionControl:true,antialias:true
  });
  map.dragPan.disable();
  map.scrollZoom.disable();
  map.keyboard.disable();
  map.on('load',()=>{
    $('mapError').classList.add('hidden');
    map.addSource('line-753',{type:'geojson',data:routeGeo});
    map.addLayer({id:'route-shadow',type:'line',source:'line-753',paint:{'line-color':'#07111b','line-width':9,'line-opacity':.65}});
    map.addLayer({id:'route-753',type:'line',source:'line-753',paint:{'line-color':'#ffd21c','line-width':5,'line-opacity':.9,'line-dasharray':[1.5,1]}});
    map.addSource('bus-stops',{type:'geojson',data:{type:'FeatureCollection',features:stops.map((s,i)=>({type:'Feature',properties:{name:s.name,index:i},geometry:{type:'Point',coordinates:[s.lon,s.lat]}}))}});
    map.addLayer({id:'stop-halo',type:'circle',source:'bus-stops',paint:{'circle-radius':10,'circle-color':'rgba(255,210,28,.2)','circle-stroke-width':2,'circle-stroke-color':'#ffd21c'}});
    map.addLayer({id:'stop-labels',type:'symbol',source:'bus-stops',layout:{'text-field':['get','name'],'text-size':11,'text-offset':[0,1.6]},paint:{'text-color':'#101820','text-halo-color':'#fff','text-halo-width':2}});
    busMarker=new maplibregl.Marker({element:createBus(),rotationAlignment:'map'}).setLngLat([state.lon,state.lat]).addTo(map);
  });
  map.on('error',()=>{if(!map.loaded())$('mapError').textContent='Kartenverbindung wird erneut aufgebaut …'});
}

addEventListener('keydown',e=>{state.keys[e.code]=true;if(['Space','KeyE','KeyC','ArrowUp','ArrowDown','ArrowLeft','ArrowRight'].includes(e.code))e.preventDefault();if(e.code==='KeyE'&&!e.repeat)toggleDoors();if(e.code==='KeyC'&&!e.repeat){state.cam=(state.cam+1)%3;toast('Kamera gewechselt',['Verfolgerkamera','Nahansicht','Übersicht'][state.cam])}});
addEventListener('keyup',e=>state.keys[e.code]=false);
$('startButton').onclick=()=>{state.started=true;$('startScreen').classList.add('hidden');toast('Dienst gestartet','Nächster Halt: Hechingen Marktplatz')};
$('pause').onclick=()=>{state.paused=!state.paused;$('pause').textContent=state.paused?'▶':'Ⅱ'};
$('sound').onclick=()=>{$('sound').classList.toggle('active');toast('Fahrgastinformation','Nächster Halt: '+stops[state.stop].name)};

function metres(a,b){
  const y=(b.lat-a.lat)*111320;
  const x=(b.lon-a.lon)*111320*Math.cos(a.lat*Math.PI/180);
  return Math.hypot(x,y);
}
function toggleDoors(){if(Math.abs(state.speed)>1)return toast('Türen verriegelt','Halte den Bus zuerst vollständig an.');state.doors=!state.doors;$('doorState').classList.toggle('active',state.doors);if(state.doors&&metres(state,stops[state.stop])<55){state.passengers=Math.min(46,state.passengers+2+Math.floor(Math.random()*5));state.stop=Math.min(stops.length-1,state.stop+1);state.score=Math.min(100,state.score+1);toast('Haltestelle bedient','Fahrgäste sind ein- und ausgestiegen.')}}
function toast(a,b){const el=$('toast');el.innerHTML=`<b>${a}</b><span>${b}</span>`;el.classList.add('show');clearTimeout(toast.t);toast.t=setTimeout(()=>el.classList.remove('show'),3200)}

function update(dt){
  if(!state.started||state.paused)return;
  const gas=state.keys.KeyW||state.keys.ArrowUp,reverse=state.keys.KeyS||state.keys.ArrowDown,brake=state.keys.Space;
  if(!state.doors){if(gas)state.speed+=20*dt;if(reverse)state.speed-=14*dt}
  state.speed*=Math.pow(brake?.07:.92,dt);state.speed=Math.max(-12,Math.min(52,state.speed));if(Math.abs(state.speed)<.05)state.speed=0;
  const steer=(state.keys.KeyA||state.keys.ArrowLeft?-1:0)+(state.keys.KeyD||state.keys.ArrowRight?1:0);
  state.angle+=steer*state.speed*.7*dt;
  const distance=state.speed/3.6*dt;
  state.lat+=Math.cos(state.angle*Math.PI/180)*distance/111320;
  state.lon+=Math.sin(state.angle*Math.PI/180)*distance/(111320*Math.cos(state.lat*Math.PI/180));
  state.time+=dt/10;
  if(busMarker){busMarker.setLngLat([state.lon,state.lat]);busMarker.setRotation(state.angle)}
  if(map){const views=[{zoom:17.2,pitch:67,behind:180},{zoom:18,pitch:72,behind:180},{zoom:15.2,pitch:45,behind:0}],v=views[state.cam];map.jumpTo({center:[state.lon,state.lat],zoom:v.zoom,pitch:v.pitch,bearing:state.angle-v.behind})}
  updateHud();drawMini();
}
function updateHud(){const target=stops[state.stop],d=Math.round(metres(state,target));$('speed').textContent=String(Math.round(Math.abs(state.speed))).padStart(2,'0');$('gear').textContent=state.speed>1?'D':state.speed<-.5?'R':'N';$('distance').textContent=d>999?(d/1000).toFixed(1)+' km':d+' m';$('stopName').textContent=target.name;$('stopCounter').textContent=`${state.stop+1} von ${stops.length} Haltestellen`;$('routeProgress').style.width=`${state.stop/(stops.length-1)*100}%`;$('passengerCount').textContent=state.passengers;$('rating').textContent=state.score+'%';$('satisfaction').style.width=state.score+'%';$('clock').textContent=`${String(Math.floor(state.time/60)%24).padStart(2,'0')}:${String(Math.floor(state.time%60)).padStart(2,'0')}`;$('doorState').textContent=state.doors?'TÜREN GEÖFFNET':'TÜREN GESCHLOSSEN';$('brakeState').classList.toggle('active',state.keys.Space)}

function drawMini(){
  const w=mini.width,h=mini.height,scale=21000;mctx.clearRect(0,0,w,h);mctx.fillStyle='#142832';mctx.fillRect(0,0,w,h);mctx.save();mctx.translate(w/2,h/2);mctx.lineCap='round';mctx.lineJoin='round';
  const point=s=>[(s.lon-state.lon)*scale,(state.lat-s.lat)*scale];
  mctx.beginPath();stops.forEach((s,i)=>{const [x,y]=point(s);i?mctx.lineTo(x,y):mctx.moveTo(x,y)});mctx.strokeStyle='#455e69';mctx.lineWidth=8;mctx.stroke();mctx.strokeStyle='#ffd21c';mctx.lineWidth=2;mctx.stroke();
  stops.forEach((s,i)=>{const [x,y]=point(s);mctx.beginPath();mctx.arc(x,y,i===state.stop?5:3,0,Math.PI*2);mctx.fillStyle=i===state.stop?'#fff':'#ffd21c';mctx.fill()});
  mctx.rotate(state.angle*Math.PI/180);mctx.fillStyle='#ffd21c';mctx.beginPath();mctx.moveTo(0,-8);mctx.lineTo(5,7);mctx.lineTo(-5,7);mctx.closePath();mctx.fill();mctx.restore();
}

let last=performance.now();function loop(now){const dt=Math.min(.04,(now-last)/1000);last=now;update(dt);requestAnimationFrame(loop)}
initMap();updateHud();drawMini();loop(last);
