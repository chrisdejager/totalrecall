#Dit is het programma dat de data 'replayed'
require 'rubygems'
require 'ruby-osc'

include OSC

client = Client.new 9090
start = Time.now

File.open('chris.txt',"r").each_line do |line|
	values = line.split(' ')
	timestamp = values[0].to_f
	key = values[1]

	while true
		delta = Time.now - start
		if(delta > timestamp)
			if key == "/muse/eeg/raw" 
				client.send Message.new('/muse/eeg/raw',values[3].to_f,values[5].to_f,values[7].to_f,values[9].to_f)
			end
			break
		end
                sleep(0.0001)
	end
end
