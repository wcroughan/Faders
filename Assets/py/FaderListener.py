import UdpComms as U
import time
import socket

# Create UDP socket to use for sending (and receiving)
sock = U.UdpComms(udpIP="127.0.0.1", portTX=8000, portRX=8001, enableRX=True, suppressWarnings=True)
# sockToWindows = U.UdpComms(udpIP="192.168.1.20", portTX=8003, portRX=8002,
#    enableRX=True, suppressWarnings=False)

sockToWindows = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
WINDOWS_IP = "192.168.1.20"
WINDOWS_PORT = 9443

while True:
    data = sock.ReadReceivedData()  # read data

    if data != None:  # if NEW data has been received since last ReadReceivedData function call
        print(data)  # print new received data
        if data.startswith("FC "):
            ds = data.split(' ')
            sockToWindows.sendto(bytes("VC {}".format(ds[2]), 'utf-8'), (WINDOWS_IP, WINDOWS_PORT))

    time.sleep(0.05)
