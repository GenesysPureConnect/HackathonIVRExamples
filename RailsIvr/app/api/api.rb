require 'grape'

module API
  class Base < Grape::API
    mount API::IvrEndpoint => '/'
  end
end
