from lib2to3.refactor import get_fixers_from_package
import os, sys
from ikpy.chain import Chain
from scipy.spatial.transform.rotation import Rotation as R
import numpy as np

def variables_to_transform(args):
    if len(args) != 7:
        print("wrong number of args")
        return
    
    raw = args[1:8]
    position = raw[0:3]
    rotation = R.from_euler('xyz',raw[3:6],degrees=True).as_matrix()
    return position,rotation

def convert_to_degrees(angles):
    degs = []
    for i in range(len(angles)):
        degs.append(np.rad2deg(angles[i]))
    return degs

def convert_to_radians(angles):
    rads = []
    for i in range(len(angles)):
        rads.append(np.deg2rad(angles[i]))
    return rads

def get_ik_result(pos,rot):
    script_path = os.path.dirname(os.path.abspath(__file__))
    chain = Chain.from_urdf_file(script_path+"//robot.urdf")
    chain.active_links_mask = [False,True,True,True,True,True,True,False]
    angles = chain.inverse_kinematics(target_position=pos,target_orientation=rot,orientation_mode="all")
    return angles


if __name__ == "__main__":
    n = sys.argv
    position,rotation = variables_to_transform(n)
    angles = convert_to_degrees(get_ik_result(position,rotation))
    print(angles[1:7])
    
    