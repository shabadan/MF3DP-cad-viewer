$(document).ready(function(){

  /*** XML3D MODIFICATIONS ***/

  /* Show/hide Arrows and planes */
  $('#btnArrows').click(function(){
    if($('#indexZarrow').html() !== '0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0'){
      $("#indexXarrow").html("0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0");
      $("#indexYarrow").html("0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0");
      $("#indexZarrow").html("0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0");
    } else {
      $("#indexXarrow").html("0 1 1 0 1 2 1 3 1 4 1 5 2 4 3 4 3 5 5 2");
      $("#indexYarrow").html("0 1 1 0 1 2 1 3 1 4 1 5 2 4 3 4 3 5 5 2");
      $("#indexZarrow").html("0 1 1 0 1 2 1 3 1 4 1 5 2 4 3 4 3 5 5 2");
    };
  });

  /* Object interactivity */

  $("body").delegate('.groupFace', 'click', function(){

    /* Uncomment the following line to activate single-face selection */
    //$(".groupFace").not(this).attr("shader", "#phongBlue");

    if ($(this).attr("shader") !== "#phongBlue") {
      $(this).attr("shader", "#phongBlue");
    } else {
      $(this).attr("shader", "#phongGreen");
    };
  });

});
