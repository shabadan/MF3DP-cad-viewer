function container (viewname) {
	var that = this;
    this.socket = new socketremote();
    this.geodb = new geoDB();
	this.viewer = new viewManager(viewname);

	var init_listener = function() {
		$('#btnZoomAll').click(function(){
			that.viewer.UpdateViewPosition(that.geodb);
		});

		/* Change Views */
		$('#btnXYpos').click(function(){
			that.viewer.StandardView('XY','+',that.geodb);
		});

		$('#btnXYneg').click(function(){
			that.viewer.StandardView('XY','-',that.geodb);
		 });

		$('#btnXZpos').click(function(){
			that.viewer.StandardView('XZ','+',that.geodb);
		});

		$('#btnXZneg').click(function(){
			that.viewer.StandardView('XZ','-',that.geodb);
		});

		$('#btnYZpos').click(function(){
			that.viewer.StandardView('YZ','+',that.geodb);
		});

		$('#btnYZneg').click(function(){
			that.viewer.StandardView('YZ','-',that.geodb);
		});
	};

	$(document).ready(init_listener());
}

container.prototype.addObject = function(jsonObject) {
	var solidid = this.geodb.AllocateNewSolid(null);
	var thesolid=this.geodb.SolidsDict.getData(solidid);
	var nmesh = jsonObject.ObjectMeshes.length;

	$( "#frameXML3D" ).append( "<group id='solid"+solidid+"' style=' '>" );

	for (var nm = 0; nm < nmesh; nm++){
		var nf=this.geodb.AddFaceBySolidItem(thesolid,null);
		$( "#solid"+solidid ).append ("<group id='groupFace"+nf+"' class='groupFace' shader='#phongBlue'><mesh type='triangles' id='face"+nf+"'> <int name='index' id='faceindx"+nf+"'>" +jsonObject.ObjectMeshes[nm].Triangles +"</int>  <float3 name='position' id='facepos"+nf+"'>"+jsonObject.ObjectMeshes[nm].Vertices+"</float3>  <float3 name='normal'>"+jsonObject.ObjectMeshes[nm].Vertices+"</float3></mesh></group>");
	}

	this.geodb.BBoxUpdateGlob();
  return 1;

};
