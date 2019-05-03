SELECT U.ID, TU.Descripcion TIPOUSUARIO,
U.ApellidoPaterno + ' ' + U.ApellidoMaterno + ' ' +U.Nombre NOMBRECOMPLETO, 
U.NombreUsuario USUARIO,
U.Password PASSWORD,
CASE WHEN P.Descripcion IS NULL  THEN '' ELSE P.Descripcion + '>' END +
CASE WHEN C.Descripcion IS NULL  THEN '' ELSE C.Descripcion + '>' END +
CASE WHEN T.Descripcion IS NULL  THEN '' ELSE T.Descripcion + '>' END +
CASE WHEN Pz.Descripcion IS NULL  THEN '' ELSE Pz.Descripcion + '>' END +
CASE WHEN Z.Descripcion IS NULL  THEN '' ELSE Z.Descripcion + '>' END +
CASE WHEN SZ.Descripcion IS NULL  THEN '' ELSE SZ.Descripcion + '>' END +
CASE WHEN SR.Descripcion IS NULL  THEN '' ELSE SR.Descripcion END AS UBICACION,
CASE WHEN H.Descripcion IS NULL  THEN '' ELSE H.Descripcion + '>' END +
CASE WHEN CIA.Descripcion IS NULL  THEN '' ELSE CIA.Descripcion + '>' END +
CASE WHEN D.Descripcion IS NULL  THEN '' ELSE D.Descripcion + '>' END +
CASE WHEN SD.Descripcion IS NULL  THEN '' ELSE SD.Descripcion + '>' END +
CASE WHEN G.Descripcion IS NULL  THEN '' ELSE G.Descripcion + '>' END +
CASE WHEN SG.Descripcion IS NULL  THEN '' ELSE SG.Descripcion + '>' END +
CASE WHEN J.Descripcion IS NULL  THEN '' ELSE J.Descripcion END AS ORGANIZACION,
U.* 
FROM Usuario U
INNER JOIN TipoUsuario TU ON U.IdTipoUsuario = TU.Id
INNER JOIN Ubicacion Ub ON U.IdUbicacion = Ub.Id
	LEFT JOIN Pais P ON Ub.IdPais = P.Id
	LEFT JOIN Campus C ON Ub.IdCampus = C.Id
	LEFT JOIN Torre T ON Ub.IdTorre = T.Id
	LEFT JOIN Piso Pz ON Ub.IdPiso = Pz.Id
	LEFT JOIN Zona Z ON Ub.IdZona = Z.Id
	LEFT JOIN SubZona SZ ON Ub.IdSubZona = SZ.Id
	LEFT JOIN SiteRack SR ON Ub.IdSiteRack = SR.Id
INNER JOIN Organizacion O ON U.IdOrganizacion = O.Id
	LEFT JOIN Holding H ON O.IdHolding = H.Id
	LEFT JOIN Compania CIA ON O.IdCompania = CIA.Id
	LEFT JOIN Direccion D ON O.IdDireccion = D.Id
	LEFT JOIN SubDireccion SD ON O.IdSubDireccion = SD.Id 
	LEFT JOIN Gerencia G ON O.IdGerencia = G.Id
	LEFT JOIN SubGerencia SG ON O.IdSubGerencia = SG.Id
	LEFT JOIN Jefatura J ON O.IdJefatura = J.Id
