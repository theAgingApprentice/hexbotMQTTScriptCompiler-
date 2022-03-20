// *****************************************************************************************************
// This MQTTfx script for Hexbot robot created by hexbotScriptCompiler on 3/20/2022 at 16:07:03.5691190
// NOTE: Doug's robot = Hexbot3C61054ADD98, Andrew's robot = Hexbot94B97E5F4A40.
// *****************************************************************************************************
var Thread = Java.type("java.lang.Thread");
function execute() {
mybot = "Hexbot3C61054ADD98/commands"
send("NEW_FLOW")
send("NEW_FLOW")
// symdef BotID Hexbot3C61054ADD98 // symdef syntax not yet implemented in compiler.
send("Flow,1000,MLRH,10,0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0")
send("Flow,1000,MLRH,10,0,0,0, 0,2,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0")
send("FLOW,49,50")
send("Flow,1000,MLRH,10,0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0")
send("FLOW,49,50")
send("DO_FLOW,$ToeAction,$msecPerFrame")
action.setExitCode(0);
action.setResultText("done.");
out("Parameter parameter set up complete");
return action;
}
function send(cmd){
mqttManager.publish(myTWIPe, cmd);
Thread.sleep(200);
}
function out(message){
output.print(message);
}
