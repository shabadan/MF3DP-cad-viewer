 function Dictionary(){
   var dictionary = {}; // private members
   var numofelem = 0;
   var that = this;

   this.setData = function(key, val) { dictionary[key] = val; numofelem++; }
   this.getData = function(key) { return dictionary[key]; }
   this.numOfElem = function () { //privileged method: can access private properties
        return numofelem;
    };
	this.Enumerate = function () {
		var thearray =[];
		for (var key in dictionary) {
			if (dictionary.hasOwnProperty(key)) {
				thearray.push(key);
			}
		}
		return thearray;
	};
}


//
function geoDB () {
	//this.NumOfSolids = 0; // this should coincide with ListOfSolids.length: it will be useful for compsolids when implemented - ListOfSolids.length is used for now
	this.SolidsCounter = 0; // counter of solids created - it only increases - counter for the new Id
	this.NumOfFaces = 0; // this should coincide with the sum of all ListOfFacesId.length for all the solids - decreases if a face is removed
	this.FacesCounter = 0; // counter of faces created - it only increases - counter for the new Id- is a unique identifier for faces
    this.SolidsDict = new Dictionary();
	/*
	var bbxmin = Number.MAX_VALUE;
	var bbxmax = -Number.MAX_VALUE;
	var bbymin = Number.MAX_VALUE;
	var bbymax = -Number.MAX_VALUE;
	var bbzmin = Number.MAX_VALUE;
	var bbzmax = -Number.MAX_VALUE;
	*/
	this.bbmin = new XML3DVec3(Number.MAX_VALUE, Number.MAX_VALUE, Number.MAX_VALUE);
	this.bbmax = new XML3DVec3(-Number.MAX_VALUE, -Number.MAX_VALUE, -Number.MAX_VALUE);
	this.BBoxUpdateXfromArr = function(varray)   // Only visible inside geoDB()
    {
        var nel = varray.length;
		for (var i=0; i < nel; i+=3) {
			/*
			if(varray[i]<bbxmin)
				bbxmin=varray[i];
			else if (varray[i]>bbxmax)
				bbxmax=varray[i];
			*/
			if(varray[i]<this.bbmin._data[0])
				this.bbmin._data[0]=varray[i];
			else if (varray[i]>this.bbmax._data[0])
				this.bbmax._data[0]=varray[i];
		}
    }
	this.BBoxUpdateYfromArr = function(varray)   // Only visible inside geoDB()
    {
        var nel = varray.length;
		for (var i=1; i < nel; i+=3) {
			/*
			if(varray[i]<bbymin)
				bbymin=varray[i];
			else if (varray[i]>bbymax)
				bbymax=varray[i];
			*/
			if(varray[i]<this.bbmin._data[1])
				this.bbmin._data[1]=varray[i];
			else if (varray[i]>this.bbmax._data[1])
				this.bbmax._data[1]=varray[i];
		}
    }
	this.BBoxUpdateZfromArr = function(varray)   // Only visible inside geoDB()
    {
        var nel = varray.length;
		for (var i=2; i < nel; i+=3) {
			/*
			if(varray[i]<bbzmin)
				bbzmin=varray[i];
			else if (varray[i]>bbzmax)
				bbzmax=varray[i];
			*/
			if(varray[i]<this.bbmin._data[2])
				this.bbmin._data[2]=varray[i];
			else if (varray[i]>this.bbmax._data[2])
				this.bbmax._data[2]=varray[i];
		}
    }
}

geoDB.prototype.AllocateNewSolid = function(solidname) {
		try{
			var n=this.SolidsDict.numOfElem();
			this.SolidsCounter++;
			if(solidname==null)
				solidname="Solid"+(this.SolidsCounter);
			//this.ListOfSolids[n]= new SolidItem(solidname,this.SolidsCounter);
			this.SolidsDict.setData(this.SolidsCounter, new SolidItem(solidname,this.SolidsCounter));
			//this.NumOfSolids++;
			//console.log ( 'Connecting...' );
			return this.SolidsCounter; //return the solidId
		}catch(err){
			return null;
		}
	};

geoDB.prototype.AddFaceBySolidId = function(soliid) {
		try{
			var thesolid=this.SolidsDict.getData(solidid);
			return this.AddFaceBySolidItem(thesolid);
		}catch(err){
			return null;
		}
	};

geoDB.prototype.AddFaceBySolidItem = function(thesolid, facename) {
		try{
			this.FacesCounter++;
			if(facename==null)
				facename="Face"+(this.FacesCounter);
			thesolid.ListOfFacesId.setData(this.FacesCounter,new FaceItem(facename,this.FacesCounter));
			return this.FacesCounter;
		}catch(err){
			return null;
		}
	};

geoDB.prototype.BBoxUpdateGlob = function() {
		try{
			var nsolid = this.SolidsDict.numOfElem();
			var solidnames = this.SolidsDict.Enumerate();
			if(nsolid===solidnames.length) {
				for (var nm = 0; nm < nsolid; nm++) {
					var thesolid = this.SolidsDict.getData(solidnames[nm]);
					var nface = thesolid.ListOfFacesId.numOfElem();
					var facesid = thesolid.ListOfFacesId.Enumerate();
					for (var nf = 0; nf < nface; nf++) {
						var vertArray = $('#facepos'+facesid[nf]).html().split(',');
						this.BBoxUpdateXfromArr(vertArray);
						this.BBoxUpdateYfromArr(vertArray);
						this.BBoxUpdateZfromArr(vertArray);
					}
				}
			} else {
				console.log("Errore tra numOfElem e enumerate");
			}
			return 1; //return the solidId
		}catch(err){
			return null;
		}
	};

geoDB.prototype.BBoxGetLimits = function() {
		try{
			var lim =[];
			lim[0]=bbxmin;
			lim[1]=bbxmax;
			lim[2]=bbymin;
			lim[3]=bbymax;
			lim[4]=bbzmin;
			lim[5]=bbzmax;
			return lim;
		}catch(err){
			return null;
		}
	};

//////////////////////////////////

function SolidItem (displayname, id) {
		this.DisplayedName = displayname;
		this.SolidId = id;
		this.ListOfFacesId = new Dictionary();

}


 //////////////////////////////////

function FaceItem (displayname, id) {
		this.DisplayedName = displayname;
		this.FaceId = id;

}


 ///////////////////////////
