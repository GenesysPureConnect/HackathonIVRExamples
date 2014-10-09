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
                    puts 'returning welcomeplay'
                    return {:action=> 'play',
                                :message=> 'Welcome ' + remotename + ', I see that you are calling from ' + remotenumber + '  Press 1 if that is correct, otherwise press 2 followed by the pound sign',
                                :id=>'welcomeplay'
                                }
                elsif lastactionid== 'welcomeplay'
                    puts 'returning getdigits'
                    return {
                        :action=>'getdigits', #terminate with #
                        :id=>'getdigits'
                    }
                elsif lastactionid== 'getdigits'

                    if lastdigitsreceived == 1
                        puts 'returning pre transfertoqueue'
                        return {:action=> 'play',
                                    :message=> 'Great, thank you',
                                    :id=>'pretransfertoqueue'
                                    }
                    else
                        puts 'returning disconnect'
                        return {
                            :action=>'disconnect'
                        }
                    end
                elsif lastactionid== 'pretransfertoqueue'
                    puts 'returning transfer to queue'
                    return {
                        :action=>'transfertoqueue'
                    }

                end

            end
        end

    end
end
