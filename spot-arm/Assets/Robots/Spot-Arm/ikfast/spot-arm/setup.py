import setuptools
from distutils.core import setup, Extension


def main():
    setup(
        name='pyikfast_spot-arm',
        version='0.0.1',
        description='ikfast wrapper',
        author='Cyberbotics',
        author_email='support@cyberbotics.com',
        ext_modules=[Extension('pyikfast_spot-arm', ['ikfast_robot.cpp', 'pyikfast.cpp'])],
        setup_requires=['wheel']
    )


if __name__ == '__main__':
    main()
