function viewManager(id) {
	this.that = this;
	this.viewtag = "#"+id;
	this.viewtag0 = id;
	this.viewPar = {};
	this.viewPar.position = [0, 0, 400];
	this.viewPar.orientation = [0, 0, 0, 0];
	this.viewPar.fieldOfView = 0.7854;
	this.viewPar.viewdistance = 100;
}

viewManager.prototype.UpdateViewPosition = function(geodb) {
		try{
			var minpoint = geodb.bbmin;
			var maxpoint = geodb.bbmax;
			var center = minpoint.add(maxpoint);
			center = center.scale(0.5);
			var width = maxpoint.subtract(minpoint);
			width = width.scale(0.5);
			var maxw = Math.max.apply(null, width._data);
			var currentpos = $(this.viewtag).attr("position").split(" ").map(parseFloat);
			currentpos = new XML3DVec3(currentpos[0], currentpos[1], currentpos[2]);
			var diff = currentpos.subtract(center);
			var currdist = Math.sqrt(diff.dot(diff));
			var ang = $(this.viewtag).attr("fieldOfView")/2;
			var currdist = 2*maxw/Math.tan(ang);
			this.viewPar.viewdistance = currdist;
			diff = diff.normalize();
			var newpos = [center._data[0]+currdist*diff._data[0], center._data[1]+currdist*diff._data[1], center._data[2]+currdist*diff._data[2]];

			var theview = document.getElementById("viewMain");

			theview.setAttribute("position", newpos.join(" ")); // look from origin
			//theview.setUpVector(diff.cross(new XML3DVec3(0, 0, 1))); // up-vector is positive Y axis
			//theview.setUpVector(theview.getUpVector());
			theview.lookAt(center); // look at the object located at (-20, 0, 0).
		}catch(err){
			return null;
		}
	};


viewManager.prototype.StandardView = function(plane,direction,geodb) {
		try{
			var minpoint = geodb.bbmin;
			var maxpoint = geodb.bbmax;
			var center = minpoint.add(maxpoint);
			center = center.scale(0.5);
			switch (plane)
			{
			  case 'XY':
						if (direction=='+') {
							var pos = center._data[2]+this.viewPar.viewdistance;
							$(this.viewtag).attr("position", center._data[0]+" "+center._data[1]+" "+pos);
							$(this.viewtag).attr("orientation", "0 0 0 0");
						}
						else if (direction=='-') {
							var pos = center._data[2]-this.viewPar.viewdistance;
							$(this.viewtag).attr("position", center._data[0]+" "+center._data[1]+" "+pos);
							$(this.viewtag).attr("orientation", "0 1 0 3.1417");
						}
						break;
			  case 'XZ':
						if (direction=='+') {
							var pos = center._data[1]+this.viewPar.viewdistance;
							$(this.viewtag).attr("position", center._data[0]+" "+pos+" "+center._data[2]);
							$(this.viewtag).attr("orientation", "-1 0 0 1.5708");
						}
						else if (direction=='-') {
							var pos = center._data[1]-this.viewPar.viewdistance;
							$(this.viewtag).attr("position", center._data[0]+" "+pos+" "+center._data[2]);
							$(this.viewtag).attr("orientation", "1 0 0 1.5708");
						}
						break;
			  case 'YZ':
						if (direction=='+') {
							var pos = center._data[0]+this.viewPar.viewdistance;
							$(this.viewtag).attr("position", pos+" "+center._data[1]+" "+center._data[2]);
							$(this.viewtag).attr("orientation", "0 1 0 1.5708");
						}
						else if (direction=='-') {
							var pos = center._data[0]-this.viewPar.viewdistance;
							$(this.viewtag).attr("position", pos+" "+center._data[1]+" "+center._data[2]);
							$(this.viewtag).attr("orientation", "0 -1 0 1.5708");
						}
						break;
			  default:  return null
			}

		}catch(err){
			return null;
		}
	};
