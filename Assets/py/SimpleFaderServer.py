import UdpComms as U
import time

# Create UDP socket to use for sending (and receiving)
sock = U.UdpComms(udpIP="127.0.0.1", portTX=8000, portRX=8001, enableRX=True, suppressWarnings=True)

while True:
    i = input("[fader #] [val]")
    if i == "q":
        break

    sock.SendData('Sent from Python: ' + i)  # Send this string to other application

    data = sock.ReadReceivedData()  # read data

    if data != None:  # if NEW data has been received since last ReadReceivedData function call
        print(data)  # print new received data
