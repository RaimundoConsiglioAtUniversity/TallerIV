def as_list(array)

    arr = Array(array)
    s = ""

    arr.each do |elem|
        if elem == arr.first
            s += elem.to_s
         elsif elem == arr.last
             if arr.count == 2
                s += " and #{elem.to_s}"
             else
             s += ", and #{elem.to_s}"
             end
         else
             s += ", #{elem.to_s}"
         end
    end

    return s
end

def is_array(inp)

    arr = Array(inp)

    if arr.count > 1
        return true
    else
        return false
    end
end
