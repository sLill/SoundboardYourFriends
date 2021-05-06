# SoundboardYourFriends <img src="https://i.imgur.com/8FQAhfR.png" width="32"/> 
<i>Don't ever never not miss what your friends say again</i>

SoundboardYourFriends records, edits and plays back sound from any desktop audio source. Simply press the Record hotkey <img src="https://i.imgur.com/clb0yUN.png" width="18"/> to save the last 30-180 seconds of dekstop audio as a replayable wav audio file that can then be edited and played back within the application. <br/> 

<img src="https://i.imgur.com/NJMIcPk.png" width="500"/>

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
	- Select Properties, go to the Advanced tab and set the quality to match your microphone quality (typically 1 Channel 44800/44100)
5. Set VBCABLE as the input device in Steam/Discord etc.

<h2>USAGE</h2>

Buffer size, file locations, hotkey modifiers and more can be changed in the application settings <img src="https://i.imgur.com/xFbGoPV.png" width="18"/>

The gray playback button <img src="https://i.imgur.com/MNAFpTI.png" width="18"/> indicates playback to local <b>Output Devices</b> to allow for playback/editing that other users cannot hear. (Only applies to output devices that were selected with the Local property checked)

The blue playback button <img src="https://i.imgur.com/MXLZi1N.png" width="18"/> indicates global playback to all <b>Output Devices</b>

<i>* For developers: To add the DSOFile reference, run DSOFile/DSOFile_Install.bat as Administrator and then add the COM reference "DSO OLE Document Properties Reader 2.1"</i>
