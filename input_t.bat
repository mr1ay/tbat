        ::Rewrited by Tbat v:1.7.1.x
        ::Edited by SpaceAdder v:3.5.x
        ::Thanks mr1ay 
        
        @echo off
        mode 55,20
        title yes
        color f
        
        :menu
                cls
                
                if %a% == 3  (
                  8  echo answer 3
                
                ) else if %a% == 5 (
                  8  echo answer 5
                
                ) else (
                  8  echo nothing
                )
                
        
        :k
                echo test
                
                
                
        goto :endf
        ::..................................
        ::Usage:
        ::call :batboxmouse
        
        :batboxmouse
        	for /f "delims=: tokens=1,2,3" %%A in ('batbox /m') do (
        		set x=%%A
        		set y=%%B
        		set z=%%C
        	)
        	goto :eof
        
        ::..................................
        :endf
