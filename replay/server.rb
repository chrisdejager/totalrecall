#Dit is het programma dat iets doet met de data
require 'rubygems'
require 'ruby-osc'

include OSC

OSC.run do
	server = Server.new 9090

	#server.add_pattern /.*/ do |*args|       # this will match any address
	#	p "/.*/:       #{ args.join(', ') }"
	#end

	#server.add_pattern %r{foo/.*} do |*args| # this will match any /foo node
	#	p "%r{foo/.*}: #{ args.join(', ') }"
	#end

	server.add_pattern "/muse/eeg/raw" do |*args| # this will just match /foo/bar address
		p "'eeg van chris': #{ args.join(', ') }"
		#`octave scriptje #{argumenten}`
	end

	server.add_pattern "/exit" do |*args|    # this will just match /exit address
		exit
	end
end
