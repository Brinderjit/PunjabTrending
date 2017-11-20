function facebookLoginCallBack(response) {
    var accessToken;
    if(response.status=='connected'){
        $("#signInBtn").text("Sign Out");
        $("#welcomeTxt").show();
       
        accessToken = response.authResponse.accessToken;
    }
    else if(response.status=='unknown') {
        $("#signInBtn").text("Sign In");
        // window.location.href = "\\Home\\Index\\";
        accessToken = 'unknown';
    }
    
           
            $.ajax({
                url: '/AuthenticateUser/logIn/',
              
               
                data: { accessToken: accessToken, status: response.status ,identity:'facebook'},
                cache: false,
                type: "POST",
                success: function (data) {
                  // alert(data.someString);
                   

                  
                       
                    

                },
                error: function (xhr, status, error) {
                  // alert(error);
                }
            });

         window.location.reload();

       

    }