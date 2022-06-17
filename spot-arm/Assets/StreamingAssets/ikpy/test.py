from ikpy.chain import Chain
import os
import numpy as np

from scipy.spatial.transform.rotation import Rotation as R

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

script_path = os.path.dirname(os.path.abspath(__file__))
my_chain = Chain.from_urdf_file(script_path+"//robot2.urdf")
my_chain.active_links_mask = [False,True,True,True,True,True,True,False]

angles = [0,0,0,0,0,0]
angles.insert(0,0)
angles.append(0)
rads = convert_to_radians(angles)

position = my_chain.forward_kinematics(rads)
# print(position)

transform = convert_to_transform(position)
print(transform)

position = transform[0:3]
rotation = R.from_euler('xyz',transform[3:6],degrees=True).as_matrix()


angles = my_chain.inverse_kinematics(target_position=position,target_orientation=rotation,orientation_mode="all")
print(convert_to_degrees(angles)[1:7])

print(convert_to_transform(my_chain.forward_kinematics(angles)))
