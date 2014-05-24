#!/bin/bash

if [ -z $1 ]
then
  echo Provide path to buffer_bci *with* trailing slash
  exit 1
fi

cd $1dataAcq/

# start buffer
./startBuffer.sh &

# start osc server
oscport=9090
oscdevice=92E6
# 1) run the OSC -> ft_buffer converter with parameters for the MUSE  !in the background!
#    This will then wait for data from the MUSE and connection to the buffer
java -cp buffer/java/Buffer.jar:osc/JavaOSC.jar:osc osc2ft /muse/eeg/raw:${oscport} localhost:1972 4 500 1 10

