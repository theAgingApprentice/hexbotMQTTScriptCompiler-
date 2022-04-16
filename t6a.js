// *****************************************************************************************************
// This MQTTfx script for Hexbot robot created by hexbotScriptCompiler on 4/15/2022 at 22:25:05.9285368
// NOTE: Doug's robot = Hexbot3C61054ADD98, Andrew's robot = Hexbot94B97E5F4A40.
// *****************************************************************************************************
var Thread = Java.type("java.lang.Thread");
function execute() {
mybot = "Hexbot3C61054ADD98/commands"
send("NEW_FLOW")
send("Flow, 1,MLRH,10,0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0")
// symdef BotID Hexbot3C61054ADD98 // symdef syntax not yet implemented in compiler.
send("Flow,500,MLC,10,0,0,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0")
send("Flow,0,MCS,1,0,0,0")
send("Flow,500,MLC,10,0,0,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-6.6,0")
send("Flow,500,MLC,10,0,0,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,17.78,-6.6,0")
send("Flow,500,MLC,10,0,0,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,17.78,-6.6,-4")
send("Flow,500,MLC,10,0,0,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,17.78,-6.6,0")
send("Flow,500,MLC,10,0,0,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,9.78,-10.6,0")
send("Flow,500,MLC,10,0,0,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-10.6,0 ,13.78,-14.6,0")
send("Flow,0,MCE,1,0,0,0")
send("Flow,0,DC,1,4,0,0,0")
send("DO_FLOW,49,50")
action.setExitCode(0);
action.setResultText("done.");
out("Parameter parameter set up complete");
return action;
}
function send(cmd){
mqttManager.publish(mybot, cmd);
Thread.sleep(200);
}
function out(message){
output.print(message);
}
