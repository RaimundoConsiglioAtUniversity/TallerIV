def funct (datum)
    s = ""
    if datum.Name
        s+= "#{datum.Name.to_s}, "
    end
    if datum.Genre
        s+= "#{datum.Genre.to_s}, "
    end
    if datum.Genre
        s+= "#{datum.Genre.to_s}, "
    end
    if datum.Genre
        s+= "#{datum.Genre.to_s}, "
    end
    if datum.Platforms && (datum.Platforms.to_s.include? "Switch")
        s+= "o, "
    end
    if datum.Platforms && (datum.Platforms.to_s.include? "DS" || "Wii")
        s+= "o, "
    end
    if datum.Platforms && (datum.Platforms.to_s.include? "Windows" || "PC")
        s+= "o, "
    end
    if datum.Platforms && (datum.Platforms.to_s.include? "Linux")
        s+= "o, "
    end
    if datum.Platforms && (datum.Platforms.to_s.include? "Mac" || "OSX")
        s+= "o, "
    end
    if datum.Platforms && (datum.Platforms.to_s.include? "iPad")
        s+= "o, "
    end
    if datum.Platforms && (datum.Platforms.to_s.include? "iOS")
        s+= "o, "
    end
    if datum.Platforms && (datum.Platforms.to_s.include? "Android")
        s+= "o, "
    end
    if datum.Platforms && (datum.Platforms.to_s.include? "ation 3" || "ation 2")
        s+= "o, "
    end
    if datum.Platforms && (datum.Platforms.to_s.include? "ation 4")
        s+= "o, "
    end
    if datum.Platforms && (datum.Platforms.to_s.include? "ation 5")
        s+= "o, "
    end
    if datum.Platforms && (datum.Platforms.to_s.include? "360")
        s+= "o, "
    end
    if datum.Platforms && (datum.Platforms.to_s.include? "One")
        s+= "o, "
    end
    if datum.Platforms && (datum.Platforms.to_s.include? "Series")
        s+= "o, "
    end
    if datum.Platforms && (datum.Platforms.to_s.include? "Stadia")
        s+= "o, "
    end
    s += ", "
    if datum.Modes && (datum.Modes.to_s.include? "Single")
        s+= "o, "
    end
    if datum.Modes && (datum.Modes.to_s.include? "Online")
        s+= "o, "
    end
    if datum.Modes && (datum.Modes.to_s.include? "Local")
        s+= "o, "
    end
    if datum.Price
        s+= "#{datum.Price.to_s}, "
    end
    if datum.DLC && (datum.DLC.to_s.include? "Yes")
        s+= "o, "
    end
    if datum.IAP && (datum.IAP.to_s.include? "Yes")
        s+= "o, "
    end
    if datum.Subscriptions && (datum.Subscriptions.to_s.include? "Yes")
        s+= "o, "
    end
    if datum.ReleaseYear
        s+= "#{datum.ReleaseYear.to_s}, "
    end
    return s
end

def funct1
    s=""
    data.temp_data.each do |key, game|
        s += "#{funct(game).to_s}\n"
    end

    return s
end
