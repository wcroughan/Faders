import UdpComms as U
import time
import socket
import random

# Create UDP socket to use for sending (and receiving)
sock = U.UdpComms(udpIP="127.0.0.1", portTX=8000, portRX=8001, enableRX=True, suppressWarnings=True)
# sockToWindows = U.UdpComms(udpIP="192.168.1.20", portTX=8003, portRX=8002,
#    enableRX=True, suppressWarnings=False)

sockToWindows = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
WINDOWS_IP = "192.168.1.20"
WINDOWS_PORT = 9443

v1 = 0.5
v2 = 0.5
v3 = 0.9
dv1 = 0.002
dv2 = 0.005
dv3 = 0.005

while True:
    sock.SendData("AVC testkey {}".format(v1))
    sock.SendData("AVC testkey2 {}".format(v2))
    sock.SendData("AVC testkey3 {}".format(v3))
    v1 += dv1
    while v1 > 1:
        v1 -= 1
    v2 += dv2
    while v2 > 1:
        v2 -= 1
    v3 += dv3
    while v3 > 1:
        v3 -= 1
    time.sleep(0.01)
