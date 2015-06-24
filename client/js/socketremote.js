// Inizio funzioni riutilizzate dal web ------------------------------------------+
// http://phpjs.org/functions/base64_encode/
function base64_encode (data) {
  var b64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
  var o1, o2, o3, h1, h2, h3, h4, bits, i = 0,
    ac = 0,
    enc = "",
    tmp_arr = [];

  if (!data) {
    return data;
  }

  do { // pack three octets into four hexets
    o1 = data.charCodeAt(i++);
    o2 = data.charCodeAt(i++);
    o3 = data.charCodeAt(i++);

    bits = o1 << 16 | o2 << 8 | o3;

    h1 = bits >> 18 & 0x3f;
    h2 = bits >> 12 & 0x3f;
    h3 = bits >> 6 & 0x3f;
    h4 = bits & 0x3f;

    // use hexets to index into b64, and append result to encoded string
    tmp_arr[ac++] = b64.charAt(h1) + b64.charAt(h2) + b64.charAt(h3) + b64.charAt(h4);
  } while (i < data.length);

  enc = tmp_arr.join('');

  var r = data.length % 3;

  return (r ? enc.slice(0, r - 3) : enc) + '==='.slice(r || 3);

}

function base64_decode (data) {
  var b64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
  var o1, o2, o3, h1, h2, h3, h4, bits, i = 0,
    ac = 0,
    dec = "",
    tmp_arr = [];

  if (!data) {
    return data;
  }

  data += '';

  do { // unpack four hexets into three octets using index points in b64
    h1 = b64.indexOf(data.charAt(i++));
    h2 = b64.indexOf(data.charAt(i++));
    h3 = b64.indexOf(data.charAt(i++));
    h4 = b64.indexOf(data.charAt(i++));

    bits = h1 << 18 | h2 << 12 | h3 << 6 | h4;

    o1 = bits >> 16 & 0xff;
    o2 = bits >> 8 & 0xff;
    o3 = bits & 0xff;

    if (h3 == 64) {
      tmp_arr[ac++] = String.fromCharCode(o1);
    } else if (h4 == 64) {
      tmp_arr[ac++] = String.fromCharCode(o1, o2);
    } else {
      tmp_arr[ac++] = String.fromCharCode(o1, o2, o3);
    }
  } while (i < data.length);

  dec = tmp_arr.join('');

  return dec;
}
// Fine funzioni dal web ---------------------------------------------------------+






//
function socketremote () {
    this.socket = null;
    this.remotePath = "ws://localhost:8181";
	//this.remotePath = "ws://192.168.243.130:8181";
	//this.remotePath = "ws://151.42.147.92:8181";
	this.socket_init = function () {

			if(this.socket){
				this.socket=this._destroySocket(this.socket);
			}
			flag=false;
			this.socket=this._connectToServer(this.remotePath);
			if(this.socket){
				//popup.setMex(mex.onconnect);
				this._defineEvent(this.socket);
			}else{
				//alert(mex.noWS);
			}

	};
	document.getElementById("btnInputGeo").addEventListener( 'change', function ( event ) {
	//qui leggo e invio il file il file
			var sock= window.maincontainer.socket.socket;
			if(sock){
				try{
					//popup.setMex(mex.sendrequest);
					try{
						var f = event.target.files[0];
					    if (f) {
					      var r = new FileReader();
					      r.onload = function(e) {
								alert("File read!");
						      	var contents = e.target.result;
						      	var jsonOb={
												instruction		: 'cad2xml3d',
						      					name 			: f.name,
						      					type 			: f.name.split('.').pop(),
						      					size 			: f.size,
						      					contents_b64 	: base64_encode(contents)
						      				};
						      	sock.send(JSON.stringify(jsonOb));
					      };
						  r.onerror = function(e) {
							  alert("Did not read file!");
							};
					      r.readAsText(f);
					      this.value = '';
					    } else {
					    	//popup.setMex(mex.nofileload,2000);
					    	this.value = '';
					    }
					}catch(error){
						//popup.setMex(mex.nofileloaderr+'\r\n'+error.message,2000);
						this.value = '';
					}

				}catch(error){
					//popup.setMex(mex.doconnect,2000);
					this.value = '';
				}
			}else{
				//popup.setMex(mex.doconnect,2000);
				this.value = '';
			}
	} );
}
 
/*
		Se il browser supporta il Websocket restituisco un oggeto WS altrimenti null.
		La funzione richiede il parametro path per la connessione.
	*/
	socketremote.prototype._connectToServer = function(path) {
		try{
			console.log ( 'Connecting...' );
			return (!window.WebSocket)?null:new WebSocket(path);
		}catch(err){
			return null;
		}
	};



		/*
		La funzione richiede un oggetto WebSocket da inizializzare
	*/
	socketremote.prototype._defineEvent = function(objSocket){
		objSocket.onerror = function () {
			flag=true;
			//popup.setMex(mex.errorWS,3000);
		};

		objSocket.onopen = function () {
			//popup.setMex(mex.connected,3000);
			console.log('websocket opened');
			setInterval(function() {
			if (objSocket.bufferedAmount == 0) {
				var jobj = {instruction: "KeepAlive"}
				objSocket.send(JSON.stringify(jobj));
				}
			}, 10000 );
		};

		objSocket.onclose = function () {
			if(!flag){
				//popup.setMex(mex.disconnected,3000);
			}
			this.socket=null;
		}

		objSocket.onmessage = function (evt) {
            var jtemp = JSON.parse(evt.data);
			//switch(evt.data){
			switch(jtemp.instruction){
				case 'wait':
					//popup.setMex(mex.endsending);
				break;
				case 'xml3dplot':
					//popup.setMex(mex.ondata);
					paintMesh(evt.data);
				break;
				case 'plot_mesh':
					//popup.setMex(mex.ondata);
					//_paintMesh(evt.data);
				break;
				default:
					//menubar.mesh???
			}
		}
	};







	socketremote.prototype._destroySocket = function(mysock){
		try{
			mysock.onclose 	= function () {};
			mysock.onerror 	= function () {};
			mysock.onopen 	= function () {};
			mysock.onmessage = function () {};
    		mysock.close();
		}catch(error){
			//
		}
		mysock=null;
		return mysock;
	};


	/*
		Carica nella scena i dati passati come oggetto json
	*/
	function paintMesh(jsonObj) {

		try{
			var JSobj 		= JSON.parse(jsonObj),
				nnow		= new Date().getTime();




			switch(JSobj.extension){
				case 'xml3d':
					var contents = base64_decode(JSobj.contents_b64);
					var jsonObject =  JSON.parse(contents);
					window.maincontainer.addObject(jsonObject);
					/*
					var nmesh = jsonObject.ObjectMeshes.length;
					$( "#frameXML3D" ).append( "<group id='mesh2group' shader='#phongGreen' >" );
					for (var nm = 0; nm < nmesh; nm++){
						$( "#mesh2group" ).append ("<mesh type='triangles' id='face'"+nm+"> <int name='index'>" +jsonObject.ObjectMeshes[nm].Triangles +"</int>  <float3 name='position'>"+jsonObject.ObjectMeshes[nm].Vertices+"</float3>  <float3 name='normal'>"+jsonObject.ObjectMeshes[nm].Vertices+"</float3></mesh>");
					}
					*/
				break;
				case 'babylon':
				break;

				default:
					//popup.setMex(mex.nofilesupp,2000);
			}

			//popup.setMex(mex.finishdata,2000);
		}catch(error){
			//popup.setMex(mex.errorparse,2000);
		}
	}
