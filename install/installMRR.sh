#!/bin/bash

######################
# Setup process
######################

## 10/9/23
#A) Write image to SD card
#B) Run GetFiles.bat (to copy all to image)
#C) Boot Pi
## & log in
##D) sudo raspi-config => enable auto  log to command line
#E) "cd /boot/InstallFiles/"
#F) sudo bash InstallSRR.sh            # to run this file 
#G) Enable remote access for database
#H) Enable remote commands (reboot) ?? not working

# configure to connect to db from outside source

######################
# setup pi
######################
#sudo adduser mrr
#sudo adduser mrr sudo

# should add new user that will require password on next log-in
# u: mrr
# p: rallypass
#sudo passwd --lock pi
#sudo deluser -remove-home pi



######################
# Update OS
######################
sudo apt-get update -y
sudo apt-get upgrade -y

######################
# Configure items
######################
cd /home/mrr/
mkdir server


######################
# copy web files
######################
#Run GetFiles on PC to create this
#sudo tar xvpf /boot/InstallFiles/PiConfig.tar -C server/
cp -R /boot/firmware/InstallFiles/* /home/mrr/server


######################
# list of apps to install
######################

#Install MySQL
sudo apt-get install mariadb-server -y 	# lamp   
#bash <(curl -sL https://raw.githubusercontent.com/node-red/linux-installers/master/deb/update-nodejs-and-nodered)
#sudo systemctl enable nodered.service

curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel STS

echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc
echo 'export PATH=$PATH:$HOME/.dotnet' >> ~/.bashrc
source ~/.bashrc

dotnet add package MySqlConnector --version 2.2.7


######################
# configure database
######################
# run sql file
sudo mysql < /boot/firmware/InstallFiles/install/SRRDatabase.sql

######################
# create startup file
######################
# copy files to other locations
##cp /boot/InstallFiles/startup.sh /.config/lxsession/LXDE/autostart

#sudo cp /boot/InstallFiles/startup.sh /etc/init.d/
#cd /etc/init.d/
#sudo chmod +x startup.sh
#sudo update-rc.d startup.sh defaults


#sudo reboot
######################
# connect to db from remote
######################
#sudo nano /etc/mysql/mariadb.conf.d/50-server.cnf
# comment out
#bind-address=127.0.0.1

#sudo reboot

dotnet add package System.Device.Gpio --source https://pkgs.dev.azure.com/dotnet/IoT/_packaging/nightly_iot_builds/nuget/v3/index.json
dotnet add package Iot.Device.Bindings --source https://pkgs.dev.azure.com/dotnet/IoT/_packaging/nightly_iot_builds/nuget/v3/index.json