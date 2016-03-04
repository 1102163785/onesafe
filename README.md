
##intro##
obfsproxy is a tool that attempts to circumvent censorship, by transforming the Tor traffic between the client and the bridge. 
This way, censors, who usually monitor traffic between the client and the bridge,
 will see innocent-looking transformed traffic instead of the actual Tor traffic.


##feature##

- C# architecture
- Base on Framework3.5
- Support Smart and global Mode 


## Server

### Install

Debian/Ubuntu

```
apt-get install gcc python-pip python-dev
pip install obfsproxy

```

### Prepare server port 

```
/usr/local/bin/obfsproxy --data-dir=/tmp/scramblesuit-server scramblesuit --password=FANGBINXINGFUCKYOURMOTHERSASS444 --dest=127.0.0.1:25 server 0.0.0.0:8087

```


###  start automatically during the system startup (Debian)
nano /etc/rc.local 

```
Add the following content to /etc/rc.local 

(/usr/local/bin/obfsproxy --data-dir=/tmp/scramblesuit-server scramblesuit --password=FANGBINXINGFUCKYOURMOTHERSASS444 --dest=127.0.0.1:25 server 0.0.0.0:23333 >/dev/null 2>&1 &)

```
