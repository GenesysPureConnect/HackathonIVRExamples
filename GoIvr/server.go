package main

import (
	"fmt"
	"github.com/emicklei/go-restful"
	"github.com/emicklei/go-restful/swagger"
	"log"
	"net/http"
)

type ActionRequest struct {
	Callid, Lastdigitsreceived             int
	Remotename, Remotenumber, Lastactionid string
}

type PlayActionResponse struct {
	Action, Message, Id string
}

type GetDigitsActionResponse struct {
	Action, Id string
}

type DisconnectActionResponse struct {
	Action string
}

type TransferToQueueActionResponse struct {
	Action string
}

func Register() {
	ws := new(restful.WebService)
	ws.
		Path("/").
		Consumes(restful.MIME_XML, restful.MIME_JSON).
		Produces(restful.MIME_JSON, restful.MIME_XML) // you can specify this per route as well

	ws.Route(ws.POST("/nextaction").To(nextaction).
		// docs
		Doc("Get the next action for the IVR").
		Operation("nextaction").
		Reads(ActionRequest{}).
		Writes(PlayActionResponse{}))

	restful.Add(ws)
}

func nextaction(request *restful.Request, response *restful.Response) {
	// Populate ActionRequest from request body
	actionRequest := new(ActionRequest)
	err := request.ReadEntity(&actionRequest)
	fmt.Println("Request:", actionRequest)

	// Process request
	if err == nil {
		// Determine appropriate action response
		if actionRequest.Lastactionid == "" {
			// Main menu
			responsePayload := PlayActionResponse{"play", "Thank you for calling the Google go I V R. Press 1 to continue.", "maingreeting"}
			fmt.Println("Response:", responsePayload)
			response.WriteEntity(responsePayload)
		} else if actionRequest.Lastactionid == "maingreeting" {
			// Main menu digits
			responsePayload := GetDigitsActionResponse{"getdigits", "maingreetinggetdigits"}
			fmt.Println("Response:", responsePayload)
			response.WriteEntity(responsePayload)
		} else if actionRequest.Lastactionid == "maingreetinggetdigits" &&
			actionRequest.Lastdigitsreceived == 1 {
			// Selected 1 from main menu
			responsePayload := PlayActionResponse{"play", "Great, Thank you.", "thankyou"}
			fmt.Println("Response:", responsePayload)
			response.WriteEntity(responsePayload)
		} else if actionRequest.Lastactionid == "maingreetinggetdigits" {
			// Selected anything but 1 from main menu
			responsePayload := DisconnectActionResponse{"disconnect"}
			fmt.Println("Response:", responsePayload)
			response.WriteEntity(responsePayload)
		} else if actionRequest.Lastactionid == "thankyou" {
			// Action after message from 1 from main menu
			responsePayload := TransferToQueueActionResponse{"transfertoqueue"}
			fmt.Println("Response:", responsePayload)
			response.WriteEntity(responsePayload)
		} else {
			// Something we didn't expect
			response.WriteErrorString(http.StatusInternalServerError, "Unable to determine next menu")
			return
		}

		// Done
		response.WriteHeader(http.StatusOK)
		return
	} else {
		response.WriteError(http.StatusInternalServerError, err)
	}
}

func main() {
	Register()

	// Optionally, you can install the Swagger Service which provides a nice Web UI on your REST API
	// You need to download the Swagger HTML5 assets and change the FilePath location in the config below.
	// Open http://http://tim-cic4su5.dev2000.com:8080/apidocs and enter http://http://tim-cic4su5.dev2000.com:8080/apidocs.json in the api input field.
	config := swagger.Config{
		WebServices:    restful.RegisteredWebServices(), // you control what services are visible
		WebServicesUrl: "http://http://tim-cic4su5.dev2000.com:8080",
		ApiPath:        "/apidocs.json",

		// Optionally, specifiy where the UI is located
		SwaggerPath:     "/apidocs/",
		SwaggerFilePath: "C:\\GoPath\\src\\github.com\\wordnik\\swagger-ui\\dist"}
	swagger.InstallSwaggerService(config)

	log.Printf("start listening on http://tim-cic4su5.dev2000.com:8080")
	log.Fatal(http.ListenAndServe(":8080", nil))
}
