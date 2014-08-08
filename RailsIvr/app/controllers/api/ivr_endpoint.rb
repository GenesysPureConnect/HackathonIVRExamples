module API
    class IvrEndpoint < Grape::API
        format :json

        resource :ivr do
            post '/nextaction' do
                puts params.inspect
                callid = params[:callid]
                remotename = params[:remotename]
                remotenumber = params[:remotenumber]
                lastdigitsreceived = params[:lastdigitsreceived]
                lastactionid = params[:lastactionid]

                if lastactionid == nil
                    #play welcome message
                    return {:action=> 'play',
                                :message=> 'Welcome ' + remotename + ', I see that you are calling from ' + remotenumber + '  Press 1 if that is correct, otherwise press 2.',
                                :id=>'welcomeplay'
                                }
                elsif lastactionid== 'welcomeplay'
                    return {
                        :action=>'getdigits', #terminate with #
                        :id=>'getdigits'
                    }
                elsif lastactionid== 'getdigits'

                    if lastdigitsreceived == 1
                        return {:action=> 'play',
                                    :message=> 'Great, thank you',
                                    :id=>'pretransfertoqueue'
                                    }
                    else
                        return {
                            :action=>'disconnect'
                        }
                    end
                elsif lastactionid== 'pretransfertoqueue'
                    return {
                        :action=>'transfertoqueue'
                    }
                end
            end

            post '/callended/:callId' do
                #if we were keeping things in a db, we might want to clean it up here
                puts 'Call ' + params[:callId] + ' ended.'
            end

        end

    end
end
