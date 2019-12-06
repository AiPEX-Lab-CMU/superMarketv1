#
#   Hello World server in Python
#   Binds REP socket to tcp://*:5555
#   Expects b"Hello" from client, replies with b"World"
#

import time
import zmq
import sys
import numpy as np
import io
from scipy import misc
import os
import imageio
from stl import mesh

context = zmq.Context()
socket = context.socket(zmq.REP)
socket.bind("tcp://*:5555")
totalTime = 0
averageTime = 0
print("python server started")
while True:
    #  Wait for next request from client
    message = socket.recv()
    print(len(message))
    data_type = message[0:3].decode('utf-8')
    '''
    message = message.decode('utf-8')
    print("Received request: %s" % message)
    waitTimeAndTimesCrossed = message.split(',')
    totalTime += int(waitTimeAndTimesCrossed[1])
    if int(waitTimeAndTimesCrossed[0]) != 0:
        averageTime = totalTime / int(waitTimeAndTimesCrossed[0])

    
    #  Do some 'work'.
    #  Try reducing sleep time to 0.01 to see how blazingly fast it communicates
    #  In the real world usage, you just need to replace time.sleep() with
    #  whatever work you want python to do, maybe a machine learning task?
    time.sleep(1)

    #  Send reply back to client
    #  In the real world usage, after you finish your work, send your output here
    if int(waitTimeAndTimesCrossed[0]) != 0:
        socket.send(str(averageTime).encode('utf-8'))
    else:
        socket.send("Infinity".encode('utf-8'))
    '''
    socket.send("Message Received".encode('utf-8'))
    if data_type == "000":
        print("image data is sent")
        metadata = message[0:11].decode('utf-8')
        print(metadata)
        with open("1.jpg", "wb") as f:
            f.write(message[11:])
        matrix = imageio.imread("1.jpg")
        print(matrix.shape)
        os.remove("1.jpg")
    elif data_type == "001":
        print("plaintext is sent")
    elif data_type == "002":
        print("point cloud data is sent")
        with open("1.obj", "wb") as f:
            f.write(message[11:])
        with open("1.obj", "r") as f:
            lines = f.readlines()
        vertices = []
        for line in lines:
            if len(line) > 0:
                arguments = line.split(' ')
                if len(arguments) == 4 and arguments[0] == "v":
                    vertices.append([float(arguments[1]), float(arguments[2]), float(arguments[3])])
        print(vertices)
        os.remove("1.obj")
    elif data_type == "End":
        print("Exiting")
        time.sleep(3)
        sys.exit()
