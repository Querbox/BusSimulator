const canvas = document.querySelector('#game');
const ctx = canvas.getContext('2d');
const mini = document.querySelector('#map');
const mctx = mini.getContext('2d');
const $ = id => document.getElementById(id);

const stops = [
  {name:'Hechingen Bahnhof',x:-620,y:250},{name:'Hechingen Marktplatz',x:-380,y:110},
  {name:'Hechingen Schulzentrum',x:-110,y:235},{name:'Hechingen Am Hang',x:110,y:70},
  {name:'Hechingen Zum Biegel',x:345,y:170},{name:'Boll Ortsmitte',x:510,y:-40},
  {name:'Boll Kirche',x:330,y:-245},{name:'Burg Parkplatz',x:50,y:-340},
  {name:'Burg Hohenzollern',x:-250,y:-305}
];
const state={x:-620,y:250,angle:-.5,speed:0,keys:{},started:false,paused:false,doors:false,stop:1,passengers:12,score:92,time:7*60+42,cam:0};

function resize(){canvas.width=innerWidth*devicePixelRatio;canvas.height=innerHeight*devicePixelRatio;ctx.setTransform(devicePixelRatio,0,0,devicePixelRatio,0,0)}
addEventListener('resize',resize);resize();
addEventListener('keydown',e=>{state.keys[e.code]=true;if(['Space','KeyE','KeyC'].includes(e.code))e.preventDefault();if(e.code==='KeyE'&&!e.repeat)toggleDoors();if(e.code==='KeyC'&&!e.repeat)state.cam=(state.cam+1)%2});
addEventListener('keyup',e=>state.keys[e.code]=false);
$('startButton').onclick=()=>{state.started=true;$('startScreen').classList.add('hidden');toast('Dienst gestartet','Nächster Halt: Hechingen Marktplatz')};
$('pause').onclick=()=>{state.paused=!state.paused;$('pause').textContent=state.paused?'▶':'Ⅱ'};
$('sound').onclick=()=>{$('sound').classList.toggle('active');toast('Fahrgastinformation','Nächster Halt: '+stops[state.stop].name)};

function toggleDoors(){if(Math.abs(state.speed)>1)return toast('Türen verriegelt','Halte den Bus zuerst vollständig an.');state.doors=!state.doors;$('doorState').classList.toggle('active',state.doors);if(state.doors){const d=Math.hypot(state.x-stops[state.stop].x,state.y-stops[state.stop].y);if(d<70){state.passengers=Math.min(46,state.passengers+2+Math.floor(Math.random()*5));state.stop=Math.min(stops.length-1,state.stop+1);state.score=Math.min(100,state.score+1);toast('Haltestelle bedient','Fahrgäste sind ein- und ausgestiegen.')}}}
function toast(a,b){const el=$('toast');el.innerHTML=`<b>${a}</b><span>${b}</span>`;el.classList.add('show');clearTimeout(toast.t);toast.t=setTimeout(()=>el.classList.remove('show'),3200)}

function update(dt){if(!state.started||state.paused)return;const gas=state.keys.KeyW||state.keys.ArrowUp,reverse=state.keys.KeyS||state.keys.ArrowDown,brake=state.keys.Space;if(!state.doors){if(gas)state.speed+=24*dt;if(reverse)state.speed-=18*dt}state.speed*=Math.pow(brake?.08:.91,dt);state.speed=Math.max(-12,Math.min(52,state.speed));if(Math.abs(state.speed)<.05)state.speed=0;const steer=(state.keys.KeyA||state.keys.ArrowLeft?-1:0)+(state.keys.KeyD||state.keys.ArrowRight?1:0);state.angle+=steer*state.speed*.0019*dt*60;state.x+=Math.cos(state.angle)*state.speed*dt*2.6;state.y+=Math.sin(state.angle)*state.speed*dt*2.6;state.time+=dt/10;updateHud()}
function updateHud(){const target=stops[state.stop],d=Math.round(Math.hypot(state.x-target.x,state.y-target.y));$('speed').textContent=String(Math.round(Math.abs(state.speed))).padStart(2,'0');$('gear').textContent=state.speed>1?'D':state.speed<-.5?'R':'N';$('distance').textContent=d>999?(d/1000).toFixed(1)+' km':d+' m';$('stopName').textContent=target.name;$('stopCounter').textContent=`${state.stop+1} von ${stops.length} Haltestellen`;$('routeProgress').style.width=`${state.stop/(stops.length-1)*100}%`;$('passengerCount').textContent=state.passengers;$('rating').textContent=state.score+'%';$('satisfaction').style.width=state.score+'%';$('clock').textContent=`${String(Math.floor(state.time/60)%24).padStart(2,'0')}:${String(Math.floor(state.time%60)).padStart(2,'0')}`;$('doorState').textContent=state.doors?'TÜREN GEÖFFNET':'TÜREN GESCHLOSSEN';$('brakeState').classList.toggle('active',state.keys.Space)}

function world(c,w,h,scale=1){c.save();c.translate(w/2-state.x*scale,h/2-state.y*scale);c.scale(scale,scale);c.fillStyle='#718476';c.fillRect(state.x-w/scale,state.y-h/scale,w*2/scale,h*2/scale);for(let x=-900;x<900;x+=150)for(let y=-600;y<600;y+=130){c.fillStyle=(x+y)%300?'#587267':'#637b68';c.fillRect(x+8,y+8,110,88)}c.lineCap='round';c.lineJoin='round';c.strokeStyle='#33434a';c.lineWidth=52;c.beginPath();c.moveTo(stops[0].x,stops[0].y);stops.slice(1).forEach(p=>c.lineTo(p.x,p.y));c.stroke();c.strokeStyle='#647178';c.lineWidth=46;c.stroke();c.strokeStyle='#e3d38c';c.lineWidth=2;c.setLineDash([16,14]);c.stroke();c.setLineDash([]);stops.forEach((p,i)=>{c.fillStyle=i===state.stop?'#ffd21c':'#f5f7ee';c.beginPath();c.arc(p.x,p.y,i===state.stop?10:7,0,Math.PI*2);c.fill();c.strokeStyle='#13232b';c.lineWidth=3;c.stroke()});drawBus(c);c.restore()}
function drawBus(c){c.save();c.translate(state.x,state.y);c.rotate(state.angle);c.fillStyle='rgba(0,0,0,.28)';c.fillRect(-23,-11,52,26);c.fillStyle='#f1c70d';c.fillRect(-27,-13,54,26);c.fillStyle='#172b35';c.fillRect(8,-11,16,22);c.fillStyle='#eef1dc';c.fillRect(-23,-11,25,4);c.fillRect(-23,7,25,4);c.fillStyle='#ffefb5';c.fillRect(24,-9,4,5);c.fillRect(24,4,4,5);c.restore()}
function render(){const w=innerWidth,h=innerHeight;ctx.clearRect(0,0,w,h);world(ctx,w,h,state.cam?.78:1);mctx.clearRect(0,0,190,190);mctx.save();mctx.translate(95,95);mctx.scale(.105,.105);mctx.translate(-state.x,-state.y);worldMap(mctx);mctx.restore();requestAnimationFrame(render)}
function worldMap(c){c.fillStyle='#142832';c.fillRect(state.x-1000,state.y-1000,2000,2000);c.strokeStyle='#526771';c.lineWidth=35;c.beginPath();c.moveTo(stops[0].x,stops[0].y);stops.slice(1).forEach(p=>c.lineTo(p.x,p.y));c.stroke();c.strokeStyle='#ffd21c';c.lineWidth=9;c.stroke();stops.forEach((p,i)=>{c.fillStyle=i===state.stop?'#fff':'#ffd21c';c.beginPath();c.arc(p.x,p.y,18,0,Math.PI*2);c.fill()});drawBus(c)}
let last=performance.now();function loop(now){const dt=Math.min(.04,(now-last)/1000);last=now;update(dt);requestAnimationFrame(loop)}updateHud();render();loop(last);
