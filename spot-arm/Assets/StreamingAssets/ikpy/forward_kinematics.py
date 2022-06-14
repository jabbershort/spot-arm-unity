from lib2to3.refactor import get_fixers_from_package
import os, sys
from ikpy.chain import Chain
from scipy.spatial.transform.rotation import Rotation as R
import numpy as np

def variables_to_angles(args):
    if len(args) != 7:
        print("wrong number of args")
        return
    
    angles = args[1:8]
    angles = [float(x) for x in angles]
    return angles

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

def get_fk_result(angles: list):
    script_path = os.path.dirname(os.path.abspath(__file__))
    chain = Chain.from_urdf_file(script_path+"//robot.urdf")
    chain.active_links_mask = [False,True,True,True,True,True,True,False]
    angles.insert(0,0)
    angles.append(0)
    position = chain.forward_kinematics(angles)
    return position

def convert_to_transform(mat):
    position = []
    position.append(mat[0][3])
    position.append(mat[1][3])
    position.append(mat[2][3])

    rot_mat = [mat[0][:3], mat[1][:3], mat[2][:3]]
    rot = R.from_matrix(rot_mat).as_euler('xyz',degrees=True)
    position.append(rot[0])
    position.append(rot[1])
    position.append(rot[2])
    return position

if __name__ == "__main__":
    n = sys.argv
    angles = convert_to_radians(variables_to_angles(n))
    position = convert_to_transform(get_fk_result(angles))
    print(position)
    
    