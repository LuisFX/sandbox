// https://fsharpforfunandprofit.com/posts/dependency-injection-1/


type UserId = int
type UserName = string
type EmailAddress = string

// But of course, an interface with one method is just a function type, so we could rewrite these “interfaces” as:
type LogInfo = string -> unit
type LogError = string -> unit

type DbGetEmail = UserId -> EmailAddress
type DbUpdateProfile = UserId * UserName * EmailAddress -> unit
type Notify = EmailAddress * EmailAddress -> unit

module DbService =
  let getEmail cnn user = "email@domain.com" : EmailAddress
  
  let updateProfile cnn (userId, userName, email) = () : unit


module EmailService =
  let sendEmailChangedNotification cnn (oldAddress, newAddress) = () : unit

module Logger =
  let info str = ()
  let error str = ()

type UpdateProfileRequest = {
    UserId : UserId 
    Name : UserName 
    EmailAddress : EmailAddress 
}

module CustomerUpdater = 

  let parseRequest(json:string)  = { UserId = 5; Name = "Luis"; EmailAddress = "luisfx@" }

  let updateCustomerProfile 
    (logInfo:LogInfo) 
    (logError:LogError) 
    (getEmail:DbGetEmail) 
    (updateProfile:DbUpdateProfile) 
    (notify:Notify)  
    (json: string) =
    try
      let request = parseRequest(json) 
      let currentEmail = getEmail(request.UserId)
      updateProfile(request.UserId, request.Name, request.EmailAddress)
    with
    | ex -> logError (sprintf "UpdateCustomerProfile failed: '%s'" ex.Message)

module CompositionRoot = 

  let dbConnectionString = "server=dbserver; database=mydatabase"
  let smtpConnectionString = "server=emailserver"
 
  let getEmail = 
    // partial application
    DbService.getEmail dbConnectionString 
      
  let updateProfile = 
    // partial application
    DbService.updateProfile dbConnectionString 
  
  let notify = 
    // partial application
    EmailService.sendEmailChangedNotification smtpConnectionString
 
  let logInfo = Logger.info
  let logError = Logger.error
 
  let parser = CustomerUpdater.parseRequest
 
  let withLogInfo msg f x = 
    logInfo msg
    f x

  let updateProfileWithLog = 
    updateProfile |> withLogInfo "Updated Profile"

  let notifyWithLog = 
    notify |> withLogInfo "Sending Email Changed Notification"

  let updateCustomerProfile = 
    // partial application
    CustomerUpdater.updateCustomerProfile updateProfileWithLog notifyWithLog


// test
//let ``when email changes but db update fails, expect notification email not sent`` () =

let test () =
  // --------------------
  // arrange
  // --------------------
  let getEmail _ = 
    "old@example.com"
  
  let updateProfile _ = 
    // deliberately fail
    //failwith "update failed"
      
    //pass
    ()

  let mutable notificationWasSent = false
  let notify _ = 
    // just set flag
    notificationWasSent <- true
  
  let logInfo msg = printfn "INFO: %s" msg
  let logError msg = printfn "ERROR: %s" msg
  
  let updateCustomerProfile = 
    CustomerUpdater.updateCustomerProfile logInfo logError getEmail updateProfile notify

  // -------------------- 
  // act
  // --------------------
  
  let json = """{"UserId" : "1","Name" : "Alice","EmailAddress" : "new@example.com"}"""
  updateCustomerProfile json 
  
  // --------------------
  // assert
  // --------------------
  
  if notificationWasSent then failwith "test failed"
