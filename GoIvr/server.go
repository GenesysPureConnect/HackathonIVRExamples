package main

import (
	"fmt"
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

	log.Printf("start listening on :8080")
	log.Fatal(http.ListenAndServe(":8080", nil))
}
