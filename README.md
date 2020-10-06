# SoundboardYourFriends

<h2>USAGE</h2>
SoundboardYourFriends continuosly stores the last 20 seconds of user audio into a buffer in memory until the user presses the "Record" hotkey- In which case the entire buffer is written to a replayable audio file. <br/>

The size of this buffer as well as the hotkeys and hotkey modifiers are all configurable through the application settings <img src="https://i.imgur.com/xFbGoPV.png" width="20"/>

The gray playback button <img src="https://i.imgur.com/MNAFpTI.png" width="20"/> indicates playback to local <b>Output Devices</b> to allow for playback/editing that other users cannot hear. (Only applies to output devices that were selected with the Local property checked)

The blue playback button <img src="https://i.imgur.com/MXLZi1N.png" width="20"/> indicates global playback to all <b>Output Devices</b>

<h2>TO INSTALL</h2>

1. Visit the release tab (https://github.com/sLill/SoundboardYourFriends/releases)
2. Download and run SoundboardYourFriends.msi (This is the only file you need)

<h2>AFTER INSTALL</h2>

1. Restart your computer and open <b>Sound Settings</b> &#8594; <b>Sound Control Panel</b>
3. On the Playback tab
	- Right-click the VBCABLE Input device and select Properties
	- Go to the Advanced tab and set the quality to match your headphone quality
4. On the Recording tab
	- <b>Right-click your default microphone device</b> 
	- Select properties and check the 'Listen to this device' box
	- Under the 'Playback through this device' dropdown, select the VBCABLE device 
	- <b>Right-click the VBCABLE output device</b>
	- Select Properties
	- Go to the Advanced tab and set the quality to match your microphone quality (typically 1 Channel 44800/44100)
5. Set VBCABLE as the input device in Steam/Discord etc.
