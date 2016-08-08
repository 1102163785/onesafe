#!/bin/bash
#Tomcat auto-start

cp tor /etc/init.d && cd /etc/init.d

chmod +x tor
update-rc.d tor defaults
