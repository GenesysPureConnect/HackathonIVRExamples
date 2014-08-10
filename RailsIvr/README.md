Ruby On Rails IVR example
=========================

This example provides an IVR example in Ruby on Rails. It uses the Grape gem to setup the REST endpoints which can be found in app/controllers/api/ivr_endpoint.rb and creates an IVR that looks like this.


Play Welcome Message -> Get Digits -> Play message if digits =1 and transfer to queue, otherwise disconnect
