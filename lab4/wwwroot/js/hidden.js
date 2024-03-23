function hiddenfunct(){
    let text= document.getElementById("hiddenAllDiv");
    if (text.style.visibility === "hidden") {
        document.getElementById("hidebutton").innerHTML = "Hide";        
        text.style.visibility = "visible";
        
    } else {
        text.style.visibility ="hidden ";
        document.getElementById("hidebutton").innerHTML = "Show";  
        
    }
}
function showForm(){
    let a= document.getElementById("form");
    if (a.style.visibility === "hidden") {    
         a.style.visibility = "visible";
        
    } else {
            a.style.visibility ="hidden ";       
        
    }
}
function addInputs() {
    
    var input1 = document.getElementById("input1");
    var input2 = document.getElementById("input2");
  
    
    var num1 = Number(input1.value);
    var num2 = Number(input2.value);
  
    
    var sum = num1 + num2;
  
    
    document.getElementById("result").innerHTML = "Result: " + sum;
  }