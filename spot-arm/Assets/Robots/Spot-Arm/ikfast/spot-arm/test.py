from ikpy.chain import Chain

spot_arm = Chain.from_urdf_file("robot.urdf")

print(spot_arm.forward_kinematics([0]*8))