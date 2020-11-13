// https://fsharpforfunandprofit.com/posts/dependency-injection-1/

// OO implementations with and without dependency injection
// Let’s take this use-case and do two different OO implementations, one with and one without dependency injection.

// First, let’s define the domain types that will be shared across all the implementations (OO and functional):

///////////////////////////////////////////////////
/// MICROSOFT EXAMPLE OF IMPLEMENTING AN INTERFACE.
type IPrintable =
   abstract member Print : unit -> unit

type SomeClass1(x: int, y: float) =
   interface IPrintable with
      member this.Print() = printfn "%d %f" x y
///////////////////////////////////////////////////////


type UserId = int
type UserName = string
type EmailAddress = string

type ILogger =
  abstract Info : string -> unit
  abstract Error : string -> unit

type IDbService = 
  abstract GetEmail : UserId -> EmailAddress
  abstract UpdateProfile : UserId * UserName * EmailAddress -> unit

type IEmailService = 
  abstract SendEmailChangedNotification : EmailAddress * EmailAddress -> unit

module Services =
  type DbService (dbCnn) =
    interface IDbService with
      member __.GetEmail user =
        "email@domain.com"
      member __.UpdateProfile (userId, userName, email) = ()

  type EmailService (smtpCnn) =
    interface IEmailService with
      member this.SendEmailChangedNotification (oldAddress, newAddress) =
        ()

  type Logger() =
    interface ILogger with
      member this.Info str = ()
      member this.Error str = ()



type UpdateProfileRequest = {
    UserId : UserId 
    Name : UserName 
    EmailAddress : EmailAddress 
}

type UserProfileUpdater 
  ( dbService:IDbService, 
    emailService:IEmailService, 
    logger:ILogger ) = 

  member this.ParseRequest(json:string) : UpdateProfileRequest = {UserId = 5; Name = "Luis"; EmailAddress = "luisfx@"}
  
  member this.UpdateCustomerProfile(json: string) =
    try
      let request = this.ParseRequest(json) 
      let currentEmail = dbService.GetEmail(request.UserId)
      dbService.UpdateProfile(request.UserId, request.Name, request.EmailAddress)
      logger.Info("Updated Profile")

      if currentEmail <> request.EmailAddress then
        logger.Info("Sending Email Changed Notification")
        emailService.SendEmailChangedNotification(currentEmail,request.EmailAddress)
    with
    | ex -> 
      logger.Error(sprintf "UpdateCustomerProfile failed: '%s'" ex.Message)

module CompositionRoot = 

  // read from config file for example
  let dbConnectionString = "server=dbserver; database=mydatabase"
  let smtpConnectionString = "server=emailserver"

  // construct the services
  let dbService = Services.DbService(dbConnectionString)
  let emailService = Services.EmailService(smtpConnectionString)
  let logger = Services.Logger()

  // construct the class, injecting the services
  let customerUpdater = UserProfileUpdater(dbService,emailService,logger)






