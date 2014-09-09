var http = require('http');
var address = 'notamac';
var port = 8000;
http.createServer(function (request, response) {
	console.log("*** New Request (" + request.url + ") ***");
	var body = '';
	var isParseError = false;
	
	// Get request body
	request.on('data', function(data){
		try{
			body = eval('('+data+')');
		}
		catch (err){
			isParseError = true;
			console.log('Error parsing request body: ' + err);
			return;
		}
	});

	// Invoked after body is retrieved
    request.on('end', function () {
    	// Cancel if there was a parse error
    	if (isParseError) {
			console.log('400 Bad request (1)');
			response.writeHead(400, {'Content-Type': 'text/plain'});
			response.end('Bad request');
    		return;
    	}

		console.log('body=' + JSON.stringify(body));

		// Check request URL and method
		if (request.url.toLowerCase() == '/nextaction' && request.method == 'POST'){
			var jsonResponse = '';

			// Determine appropriate action
			if (body.lastactionid == '' || body.lastactionid == undefined || body.lastactionid == null){
				jsonResponse ={
				    "action" : "play",
				    "message" : "Thank you for calling the Google go I V R. Press 1 to continue.",
				    "id" : "maingreeting"
				};
			}
			else if (body.lastactionid == 'maingreeting'){
				jsonResponse ={
				    "action" : "getdigits",
				    "id" : "maingreetinggetdigits"
				};
			}
			else if (body.lastactionid == 'maingreetinggetdigits' && body.lastdigitsreceived == '1'){
				jsonResponse ={
				    "action" : "play",
				    "message" : "Great, Thank you.",
				    "id" : "thankyou"
				};
			}
			else if (body.lastactionid == 'maingreetinggetdigits'){
				jsonResponse = {"action" : "disconnect"};
			}
			else if (body.lastactionid == 'thankyou'){
				jsonResponse ={
				    "action" : "transfertoqueue",
				};
			}
			else {
				jsonResponse = {"action" : "disconnect"};
			}

			// Send response
			console.log('response='+JSON.stringify(jsonResponse));
			response.writeHead(200, {'Content-Type': 'text/json'});
			response.end(JSON.stringify(jsonResponse));
			return;
		}
		else if (request.url.toLowerCase() == '/teapot'){
			console.log('THIS IS TEAPOT!!!');
			response.writeHead(418, {'Content-Type': 'text/plain'});
			response.end("I'm a little teapot\nshort and stout\nhere is my handle\nhere is my spout\nwhen you tip me over\nhear me shout\njust tip me over\nand pour me out");
			return;
		}

		console.log('400 Bad request (2)');
		response.writeHead(400, {'Content-Type': 'text/plain'});
		response.end('Bad request');
    });

}).listen(port, address);
console.log('Server running at http://'+address+':'+port+"/");