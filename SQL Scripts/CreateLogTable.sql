/*  Instructions:
- Create a new table called server_response_log:
  - pk (primary key,int, not null, auto incrementing)
  - StartTimeUTC (datetime,not null)
  - EndTimeUTC (datetime,not null)
  - HTTPStatusCode (int, null)
  - DataString (varchar(MAX), null)
  - Status (int, not null)
  - StatusString (varchar(MAX), null)
*/

use ae_code_challange

CREATE TABLE server_response_log (
	pk int IDENTITY (1,1) NOT NULL,    
    StartTimeUTC datetime not null,
	EndTimeUTC datetime not null,
    HTTPStatusCode int null,
    DataString varchar(MAX) null,
    Status int not null,
	StatusString varchar(MAX) null, 

	CONSTRAINT PK_server_response_log_pk PRIMARY KEY CLUSTERED (pk) 
); 

select * from server_response_log