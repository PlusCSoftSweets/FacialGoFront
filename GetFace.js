var request = require('request');
var fs = require('fs');
const sharp = require('sharp');
var face_image;
var formData = {
    // Pass a simple key-value pair
    api_key:"J2isE6Vb8xUbud3CSSEiFXqNFsuImDC4",
    // Pass data via Buffers
    api_secret: "fy1b1Iz7CAA6HB3BpQHMWl1tvG5Smdm9",
    image_file: fs.createReadStream(__dirname + '/face.jpg'),
};
request.post({url:'https://api-cn.faceplusplus.com/facepp/v3/detect',formData : formData}, function (error, response, body) {
    //console.log(body);
    if(error){
        console.log(error)
    }
    if (body === undefined){

    }else{
      res = JSON.parse(body);
    /*var faces_array = res.faces.map(one => {
      return one.face_rectangle;
    });*/
    //console.log(res.faces);
    //console.log(faces_array);
      var one_face = res.faces[0].face_rectangle;
      console.log(one_face);
      console.log(one_face.width);
      sharp("face.jpg")
      .extract({ left: one_face.left, top: one_face.top, width: one_face.width, height: one_face.height })
      .toFile("face11.png", function(err) {
        console.log(err);
      });

    }
})
