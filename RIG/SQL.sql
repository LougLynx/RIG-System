select * from ActualReceivedTLIP;
select * from ActualDetailTLIP;
select * from PlanReceiveTLIP;
select * from PlanDetailReceivedTLIP;
select * from Weekday;
select * from Supplier;


INSERT INTO `PlanDetailReceivedTLIP`
 (`PlanID`, `SupplierCode`,`DeliveryTime`, `WeekdayID`, `LeadTime`,`PlanType`, `WeekOfMonth`,`OccurrencesPerMonth`)
 VALUES
 -- -----------------------------------------------------------------------------------------------------------------------------
 -- Thứ 2:-----------------------------------------------------------------------------------------------------------------------
 (1, 'A01', '07:35:00', 1, '01:00:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '07:35:00', 1, '01:00:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending.........)
 (1, 'A03', '07:35:00', 1, '01:00:00', 'Weekly', NULL, NULL), -- HAL 
 
 (1, 'G66/G68', '06:00:00', 1, '04:00:00', 'Weekly', NULL, NULL), -- DNTH/DSTH
 
 (1, 'E25', '10:35:00', 1, '02:00:00', 'Weekly', NULL, NULL), -- Kuroda
 (1, 'D04', '10:35:00', 1, '02:00:00', 'Weekly', NULL, NULL),--  HAMADEN 
 (1, 'TD60', '10:35:00', 1, '02:00:00', 'Weekly', NULL, NULL),--  HAMADEN Trading 
 (1, 'E12', '10:35:00', 1, '02:00:00', 'Weekly', NULL, NULL),-- Sanwa VN
 (1, 'A08', '10:35:00', 1, '02:00:00', 'Weekly', NULL, NULL), -- Fukoku
 (1, 'E01', '10:35:00', 1, '02:00:00', 'Weekly', NULL, NULL), -- Kyoshin VN
 (1, 'E22', '10:35:00', 1, '02:00:00', 'Weekly', NULL, NULL), -- Ohashi Tekko
 (1, 'A07', '10:35:00', 1, '02:00:00', 'Weekly', NULL, NULL), -- NSK VN
 (1, 'B35', '10:35:00', 1, '02:00:00', 'Weekly', NULL, NULL), -- Ha Son
 
 (1, 'A01', '11:05:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '11:05:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending........)
 (1, 'A03', '11:05:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
-- (1, 'A01', '12:30:00', 1, '01:00:00', 'Weekly', NULL, NULL),-- DNJP Air (pending.............)
  
 (1, 'E15', '13:00:00', 1, '01:00:00', 'Weekly', NULL, NULL), -- Serteck
 (1, 'E19', '13:00:00', 1, '01:00:00', 'Weekly', NULL, NULL), -- Fertile
 (1, 'E05', '13:00:00', 1, '01:00:00', 'Weekly', NULL, NULL), -- Advanex
 (1, 'E20', '13:00:00', 1, '01:00:00', 'Weekly', NULL, NULL), -- Curious
 
 (1, 'A01', '13:30:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '13:30:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '13:30:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
 (1, 'I04', '14:30:00', 1, '02:00:00', 'Monthly', 2, NULL), -- Hokuriku (monthly)
 
--  (1, 'I04', '14:30:00', 1, '02:00:00', 'Monthly', 2, NULL), -- Cryomax (monthly pending..............)
 
 (1, 'A01', '15:30:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '15:30:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '15:30:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
 (1, 'A01', '16:30:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '16:30:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '16:30:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
 (1, 'E15', '16:30:00', 1, '01:00:00', 'Weekly', NULL, NULL), -- Surteck
 -- (1, 'E15', '16:30:00', 1, '01:00:00', 'Weekly', NULL, NULL), -- Nippoo (pending................)
 (1, 'D04', '16:30:00', 1, '01:00:00', 'Weekly', NULL, NULL), -- HDVN (ver 1)
 (1, 'TD60', '16:30:00', 1, '01:00:00', 'Weekly', NULL, NULL), -- HDVN trading (ver 1)
 
 
 (1, 'D04', '18:00:00', 1, '01:30:00', 'Weekly', 3, NULL), -- HDVN (ver 1)
 (1, 'TD60', '18:00:00', 1, '01:30:00', 'Weekly', 3, NULL), -- HDVN trading (ver 1)
 
 (1, 'A01', '19:00:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '19:00:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '19:00:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
 (1, 'A01', '19:30:00', 1, '01:00:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '19:30:00', 1, '01:00:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '19:30:00', 1, '01:00:00', 'Weekly', NULL, NULL), -- HAL
 (1, 'D04', '19:30:00', 1, '01:00:00', 'Weekly', NULL, NULL), -- HDVN (ver 1)
 (1, 'TD60', '19:30:00', 1, '01:00:00', 'Weekly', NULL, NULL), -- HDVN trading (ver 1)
 (1, 'E25', '19:30:00', 1, '01:00:00', 'Weekly', NULL, NULL), -- Kuroda
 
 (1, 'A01', '23:30:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '23:30:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '23:30:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
 (1, 'A01', '02:30:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '02:30:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '02:30:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
 (1, 'A01', '05:20:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '05:20:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '05:20:00', 1, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
 
 -- (1, 'A01', '00:00:00', 1, '06:00:00', 'Weekly', NULL, NULL), -- DNJP (pending.....)
 -- -------------------------------------------------------------------------------------------------------------------------------
 -- Thứ 3: -------------------------------------------------------------------------------------------------------------------
 (1, 'I24', '06:00:00', 2, '02:00:00', 'Monthly', 1, NULL), -- JONHSON ELECTRIC WORLD TRADE COMPANY
 (1, 'I06', '06:00:00', 2, '02:00:00', 'Monthly', 1, NULL), -- HOINAK LIMITTED
 (1, 'I14', '06:00:00', 2, '02:00:00', 'Monthly', 1, NULL), -- Hirata
 (1, 'I18', '06:00:00', 2, '02:00:00', 'Monthly', 1, NULL), -- TDK
 
 (1, 'A01', '07:30:00', 2, '01:00:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '07:00:00', 2, '01:00:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '07:00:00', 2, '01:00:00', 'Weekly', NULL, NULL), -- HAL
 
 (1, 'I05', '08:00:00', 2, '02:00:00', 'Weekly', NULL, NULL), -- NTA
 
 (1, 'E25', '10:30:00', 2, '02:00:00', 'Weekly', NULL, NULL), -- Kuroda
 (1, 'D04', '10:30:00', 2, '02:00:00', 'Weekly', NULL, NULL),--  HAMADEN (ver 1)
 (1, 'E12', '10:30:00', 2, '02:00:00', 'Weekly', NULL, NULL),-- Sanwa VN
 (1, 'E22', '10:30:00', 2, '02:00:00', 'Weekly', NULL, NULL),-- Ohashi Tekko VN
 
 (1, 'A01', '11:00:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '11:00:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '11:00:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
-- (1, 'A02', '10:30:00', 2, '01:30:00', 'Bi-Monthly', NULL, NULL), -- DNIA Trading 2-3/M (pending.................)
 
-- (1, 'A02', '12:30:00', 2, '01:30:00', 'Monthly', 1, NULL), -- DIAS monthly (pending.................)
 
-- (1, 'A01', '12:30:00', 2, '01:00:00', 'Weekly', NULL, NULL),-- DNJP Air (pending.............)
 
 (1, 'E15', '13:00:00', 2, '01:00:00', 'Weekly', NULL, NULL), -- Surteck
 (1, 'E19', '13:00:00', 2, '01:00:00', 'Weekly', NULL, NULL), -- Fertile
 
 (1, 'A01', '13:30:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '13:30:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '13:30:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
 (1, 'E27', '14:30:00', 2, '01:30:00', 'Weekly', NULL, NULL), -- Chuburika (HCM)
 (1, 'E13', '14:30:00', 2, '01:30:00', 'Weekly', NULL, NULL), -- Kawasaki (HCM)
 (1, 'E14', '14:30:00', 2, '01:30:00', 'Weekly', NULL, NULL), -- Sanyo (HCM)
 (1, 'E18', '14:30:00', 2, '01:30:00', 'Weekly', NULL, NULL), -- Kurabe (HCM)
 (1, 'E03', '14:30:00', 2, '01:30:00', 'Weekly', NULL, NULL), -- Meinan (HCM)
 (1, 'E02', '14:30:00', 2, '01:30:00', 'Weekly', NULL, NULL), -- Maruei (HCM)
 (1, 'E04', '14:30:00', 2, '01:30:00', 'Weekly', NULL, NULL), -- Hariki (HCM)
 (1, 'E30', '14:30:00', 2, '01:30:00', 'Weekly', NULL, NULL), -- NOK (HCM)
 -- (1, 'E27', '14:30:00', 2, '01:30:00', 'Weekly', NULL, NULL), -- G.S.Electech (HCM) (pending........)
 (1, 'E26', '14:30:00', 2, '01:30:00', 'Weekly', NULL, NULL), -- Mabuchi (HCM)
 
 (1, 'E09', '15:00:00', 2, '01:00:00', 'Weekly', NULL, NULL), -- Vina Taiyo
 (1, 'E24', '15:00:00', 2, '01:00:00', 'Weekly', NULL, NULL), -- Daito Rubber
 
 (1, 'A01', '15:30:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '15:30:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '15:30:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
 (1, 'D04', '16:00:00', 2, '01:00:00', 'Weekly', NULL, NULL), -- Hamaden (ver 1)
 -- (1, 'A02', '16:00:00', 2, '01:00:00', 'Weekly', NULL, NULL), -- TPR (pending..........)
--  (1, 'A02', '16:00:00', 2, '01:00:00', 'Weekly', NULL, NULL), -- Nippo (pending..........)
 (1, 'E15', '16:00:00', 2, '01:00:00', 'Weekly', NULL, NULL), -- Surteck 
 
 (1, 'A01', '16:30:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '16:30:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '16:30:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
 (1, 'I15', '18:00:00', 2, '01:30:00', 'Monthly', 2, NULL), -- Miruko (monthly)
 
 (1, 'A01', '19:00:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '19:00:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '19:00:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
 (1, 'A01', '19:30:00', 2, '01:00:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '19:30:00', 2, '01:00:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '19:30:00', 2, '01:00:00', 'Weekly', NULL, NULL), -- HAL
 (1, 'D04', '19:30:00', 2, '01:00:00', 'Weekly', NULL, NULL), -- Hamaden (ver 1)
 (1, 'E25', '19:30:00', 2, '01:00:00', 'Weekly', NULL, NULL), -- Kuroda  
 
 (1, 'A01', '23:30:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '23:30:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '23:30:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
 -- (1, 'E25', '22:30:00', 2, '04:30:00', 'Weekly', NULL, NULL), -- DNJP  (pending........)
 (1, 'G66/G68', '03:00:00', 2, '03:00:00', 'Weekly', NULL, NULL), -- DSTH 
 
 (1, 'A01', '02:30:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '02:30:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '02:30:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
  (1, 'A01', '05:20:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '05:20:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '05:20:00', 2, '00:30:00', 'Weekly', NULL, NULL), -- HAL
  -- -------------------------------------------------------------------------------------------------------------------------------
 -- Thứ 4: -------------------------------------------------------------------------------------------------------------------
 -- (1, 'A02', '06:00:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- DNJP (pending..........)
 
 (1, 'A01', '07:30:00', 3, '01:00:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '07:30:00', 3, '01:00:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '07:30:00', 3, '01:00:00', 'Weekly', NULL, NULL), -- HAL
 
 (1, 'E25', '10:30:00', 3, '02:00:00', 'Weekly', NULL, NULL), -- Kuroda
 (1, 'D04', '10:30:00', 3, '02:00:00', 'Weekly', NULL, NULL),--  HAMADEN (ver 1)
 (1, 'E12', '10:30:00', 3, '02:00:00', 'Weekly', NULL, NULL),-- Sanwa VN
 (1, 'E22', '10:30:00', 3, '02:00:00', 'Weekly', NULL, NULL),-- Ohashi Tekko VN
 
 (1, 'A01', '11:00:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '11:00:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '11:00:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 (1, 'A02', '11:00:00', 3, '01:30:00', 'Monthly', 2, NULL), -- DNWX
 
  -- (1, 'A02', '12:30:00', 3, '01:00:00', 'Weekly', NULL , NULL), -- DNJP Air(pending...........)
  
  (1, 'E15', '13:00:00', 3, '01:00:00', 'Weekly', NULL , NULL), -- Surteck
  (1, 'E19', '13:30:00', 3, '01:00:00', 'Weekly', NULL , NULL), -- Fertile
  
 (1, 'A01', '13:30:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '13:30:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '13:30:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
-- (1, 'A02', '14:30:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- Fujikura (pending..........)
 
 -- (1, 'A02', '15:00:00', 3, '01:30:00', 'Monthly', 2, NULL), -- Tsuchiya (pending..........)
 
  (1, 'A01', '15:30:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '15:30:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '15:30:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
  (1, 'D04', '16:00:00', 3, '01:30:00', 'Weekly', NULL, NULL), -- Hamaden (ver 1)
  (1, 'B372', '16:00:00', 3, '01:30:00', 'Weekly', NULL, NULL), -- Sertim 
  (1, 'E23', '16:00:00', 3, '01:30:00', 'Weekly', NULL, NULL), -- Micro Techno
  (1, 'E06', '16:00:00', 3, '01:30:00', 'Weekly', NULL, NULL), -- Atarih
  (1, 'E29', '16:00:00', 3, '01:30:00', 'Weekly', NULL, NULL), -- Vietinak
  -- (1, 'B372', '16:00:00', 3, '01:30:00', 'Weekly', NULL, NULL), -- Nippo (Pending........)
  (1, 'E15', '16:00:00', 3, '01:30:00', 'Weekly', NULL, NULL), -- Surteck 
 
 -- (1, 'D13', '18:00:00', 3, '02:00:00', 'Bi-Monthly', NULL, NULL), -- DNMX 2-3/M (pending.....................)
  
  (1, 'A01', '19:00:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
  (1, 'A02', '19:00:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
  (1, 'A03', '19:00:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- HAL
  
  (1, 'H91', '19:30:00', 3, '02:30:00', 'Monthly', 3, NULL), -- Okaya (monthly)
  (1, 'H96', '19:30:00', 3, '02:30:00', 'Monthly', 2, NULL), -- Itomol (monthly)
  (1, 'G03', '19:30:00', 3, '02:30:00', 'Monthly', 3, NULL), -- SWS (monthly) 
  
  (1, 'A01', '19:30:00', 3, '01:00:00', 'Weekly', NULL, NULL), -- Ohara
  (1, 'A02', '19:30:00', 3, '01:00:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
  (1, 'A03', '19:30:00', 3, '01:00:00', 'Weekly', NULL, NULL), -- HAL
  (1, 'D04', '19:30:00', 3, '01:00:00', 'Weekly', NULL, NULL), -- Hamaden (ver 1)
  (1, 'E25', '19:30:00', 3, '01:00:00', 'Weekly', NULL, NULL), -- Kuroda
  
  (1, 'I17', '22:30:00', 3, '02:30:00', 'Monthly', 1, NULL), -- Nissin (monthly)
  (1, 'H93', '22:30:00', 3, '02:00:00', 'Monthly', 3, NULL), -- Sanko (monthly)
  
  (1, 'A01', '23:30:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
  (1, 'A02', '23:30:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
  (1, 'A03', '23:30:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- HAL
  
  (1, 'I10', '00:30:00', 3, '01:00:00', 'Monthly', 3, NULL), -- SCHAEFFLER (monthly)
  
  (1, 'G66/G68', '02:00:00', 3, '03:30:00', 'Weekly', NULL , NULL), -- DNTH/DSTH 
  
   (1, 'A01', '02:30:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
  (1, 'A02', '02:30:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
  (1, 'A03', '02:30:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- HAL
  
   (1, 'A01', '05:20:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
  (1, 'A02', '05:20:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
  (1, 'A03', '05:20:00', 3, '00:30:00', 'Weekly', NULL, NULL), -- HAL
  
   -- -------------------------------------------------------------------------------------------------------------------------------
 -- Thứ 5: -------------------------------------------------------------------------------------------------------------------
 -- (1, 'A02', '07:00:00', 4, '03:00:00', 'Weekly', NULL, NULL), -- DNKA (pending..........)
  
  (1, 'A01', '07:30:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
  (1, 'A02', '07:30:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
  (1, 'A03', '07:30:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- HAL

 (1, 'E25', '10:30:00', 4, '02:00:00', 'Weekly', NULL, NULL), -- Kuroda
 (1, 'D04', '10:30:00', 4, '02:00:00', 'Weekly', NULL, NULL),--  HAMADEN (ver 1)
 (1, 'E12', '10:30:00', 4, '02:00:00', 'Weekly', NULL, NULL),-- Sanwa VN
 (1, 'E22', '10:30:00', 4, '02:00:00', 'Weekly', NULL, NULL),-- Ohashi Tekko VN
 
 (1, 'A01', '11:00:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
  (1, 'A02', '11:00:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
  (1, 'A03', '11:00:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- HAL
  
 -- (1, 'A02', '12:30:00', 4, '01:00:00', 'Weekly', NULL , NULL), -- DNJP Air(pending...........)
  
  (1, 'E15', '13:00:00', 4, '01:00:00', 'Weekly', NULL , NULL), -- Surteck
  (1, 'E19', '13:30:00', 4, '01:00:00', 'Weekly', NULL , NULL), -- Fertile
  
 (1, 'A01', '13:30:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '13:30:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '13:30:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
--  (1, 'A02', '14:30:00', 4, '02:00:00', 'Bi-Monthly', NULL, NULL), -- DNTH/DELT(pending..........)
  
 (1, 'A01', '15:30:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '15:30:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '15:30:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
 (1, 'D04', '16:00:00', 4, '01:30:00', 'Weekly', NULL, NULL), -- Hamaden (ver 1)
 (1, 'E21', '16:00:00', 4, '01:30:00', 'Weekly', NULL, NULL), -- (TOYO) DRILIBE VN
 -- (1, 'A01', '16:00:00', 4, '01:30:00', 'Weekly', NULL, NULL), -- Nippo (pending.....)
 (1, 'E15', '16:00:00', 4, '01:30:00', 'Weekly', NULL, NULL), -- Surteck

  (1, 'A01', '16:30:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '16:30:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '16:30:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
-- (1, 'A02', '18:00:00', 4, '01:30:00', 'Weekly', NULL, NULL), -- Toyota Tsusho JP (pending..........)
  
 (1, 'A01', '19:00:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '19:00:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '19:00:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
 (1, 'A01', '19:30:00', 4, '01:00:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '19:30:00', 4, '01:00:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '19:30:00', 4, '01:00:00', 'Weekly', NULL, NULL), -- HAL
 (1, 'D04', '19:30:00', 4, '01:00:00', 'Weekly', NULL, NULL), -- Hamaden (ver 1)
 (1, 'E25', '19:30:00', 4, '01:00:00', 'Weekly', NULL, NULL), -- Kuroda
    
    -- DMNS (pending.........)
   
 (1, 'A01', '23:00:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '23:00:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '23:00:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
 -- (1, 'A02', '22:30:00', 4, '04:30:00', 'Weekly', NULL, NULL), -- DNJP (pending...........)
  
 (1, 'A01', '02:30:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '02:30:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '02:30:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
 (1, 'A01', '05:20:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '05:20:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '05:20:00', 4, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
 -- DNTH/DSTH (pending..........)
 
    -- -------------------------------------------------------------------------------------------------------------------------------
 -- Thứ 6: -------------------------------------------------------------------------------------------------------------------
  (1, 'G05', '06:00:00', 5, '02:30:00', 'Weekly', NULL, NULL), -- Yamada
  
  (1, 'A01', '07:30:00', 5, '01:00:00', 'Weekly', NULL, NULL), -- Ohara
  (1, 'A02', '07:30:00', 5, '01:00:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
  (1, 'A03', '07:30:00', 5, '01:00:00', 'Weekly', NULL, NULL), -- HAL
  
    -- DNJP trading (pending....)
    
  (1, 'E25', '10:30:00', 5, '02:00:00', 'Weekly', NULL, NULL), -- Kuroda
 (1, 'D04', '10:30:00', 5, '02:00:00', 'Weekly', NULL, NULL),--  HAMADEN (ver 1)
 (1, 'TD60', '10:30:00', 5, '02:00:00', 'Weekly', NULL, NULL),--  HAMADEN trading (ver 1)
 (1, 'E12', '10:30:00', 5, '02:00:00', 'Weekly', NULL, NULL),-- Sanwa VN
 (1, 'E22', '10:30:00', 5, '02:00:00', 'Weekly', NULL, NULL),-- Ohashi Tekko VN
 
 (1, 'A01', '11:00:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '11:00:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '11:00:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- HAL 
  
 --  (1, 'A02', '12:30:00', 5, '01:00:00', 'Weekly', NULL , NULL), -- DNJP Air(pending...........)
  
  (1, 'E15', '13:00:00', 5, '01:00:00', 'Weekly', NULL , NULL), -- Surteck
  (1, 'E19', '13:30:00', 5, '01:00:00', 'Weekly', NULL , NULL), -- Fertile
  
 (1, 'A01', '13:30:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '13:30:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '13:30:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- HAL
  
-- DNMY Trading (pending.......)
  
   (1, 'E27', '14:30:00', 5, '01:30:00', 'Weekly', NULL, NULL), -- Chuburika (HCM)
 (1, 'E13', '14:30:00', 5, '01:30:00', 'Weekly', NULL, NULL), -- Kawasaki (HCM)
 (1, 'E14', '14:30:00', 5, '01:30:00', 'Weekly', NULL, NULL), -- Sanyo (HCM)
 (1, 'E18', '14:30:00', 5, '01:30:00', 'Weekly', NULL, NULL), -- Kurabe (HCM)
 (1, 'E03', '14:30:00', 5, '01:30:00', 'Weekly', NULL, NULL), -- Meinan (HCM)
 (1, 'E02', '14:30:00', 5, '01:30:00', 'Weekly', NULL, NULL), -- Maruei (HCM)
 (1, 'E04', '14:30:00', 5, '01:30:00', 'Weekly', NULL, NULL), -- Hariki (HCM)
 (1, 'E30', '14:30:00', 5, '01:30:00', 'Weekly', NULL, NULL), -- NOK (HCM)
-- (1, 'E27', '14:30:00', 5, '01:30:00', 'Weekly', NULL, NULL), -- G.S.Electech (HCM) (pending........)
 (1, 'E26', '14:30:00', 5, '01:30:00', 'Weekly', NULL, NULL), -- Mabuchi (HCM)
 
  (1, 'A01', '15:30:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '15:30:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '15:30:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
  (1, 'D04', '16:00:00', 5, '01:30:00', 'Weekly', NULL, NULL), -- Hamaden (ver 1)
  (1, 'TD60', '16:00:00', 5, '01:30:00', 'Weekly', NULL, NULL), -- Hamaden trading (ver 1)
--  (1, 'A01', '16:00:00', 5, '01:30:00', 'Weekly', NULL, NULL), -- Nippo (pending.....)
 (1, 'E15', '16:00:00', 5, '01:30:00', 'Weekly', NULL, NULL), -- Surteck
  (1, 'E23', '16:00:00', 5, '01:30:00', 'Weekly', NULL, NULL), -- Micro Techno
  
  (1, 'A01', '16:30:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '16:30:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '16:30:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
   (1, 'A01', '19:00:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '19:00:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '19:00:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
  (1, 'A01', '19:30:00', 5, '01:00:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '19:30:00', 5, '01:00:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '19:30:00', 5, '01:00:00', 'Weekly', NULL, NULL), -- HAL
 (1, 'D04', '19:30:00', 5, '01:00:00', 'Weekly', NULL, NULL), -- Hamaden (ver 1)
  (1, 'TD60', '19:30:00', 5, '01:00:00', 'Weekly', NULL, NULL), -- Hamaden trading ((ver 1)
 (1, 'E25', '19:30:00', 5, '01:00:00', 'Weekly', NULL, NULL), -- Kuroda
 
  (1, 'B222', '20:00:00', 5, '02:00:00', 'Monthly', 3, NULL), -- ARMOR WEATHYSHINE
  -- NOK monthly (pending.....)
 (1, 'G02', '20:00:00', 5, '02:00:00', 'Monthly', 3, NULL), -- Hitachimetal
   
 (1, 'G06', '20:00:00', 5, '02:00:00', 'Weekly', NULL, NULL), -- Daiki
    
 (1, 'H98', '22:30:00', 5, '01:30:00', 'Monthly', 2, NULL), -- Mitsubishi
    
 (1, 'A01', '23:30:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '23:30:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '23:30:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
  (1, 'D08', '00:00:00', 5, '01:30:00', 'Weekly', NULL, NULL), -- Aine
  
  (1, 'G66/G68', '00:00:00', 5, '06:00:00', 'Weekly', NULL, NULL), -- DNTH
  
 (1, 'A01', '02:30:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '02:30:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '02:30:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
  (1, 'A01', '05:20:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
 (1, 'A02', '05:20:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
 (1, 'A03', '05:20:00', 5, '00:30:00', 'Weekly', NULL, NULL), -- HAL
 
     -- -------------------------------------------------------------------------------------------------------------------------------
 -- Thứ 7: -------------------------------------------------------------------------------------------------------------------
  (1, 'A01', '07:30:00', 6, '01:00:00', 'Weekly', NULL, NULL), -- Ohara
  (1, 'A02', '07:30:00', 6, '01:00:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
  (1, 'A03', '07:30:00', 6, '01:00:00', 'Weekly', NULL, NULL), -- HAL
  
  (1, 'E25', '10:30:00', 6, '02:00:00', 'Weekly', NULL, NULL), -- Kuroda
  (1, 'D04', '10:30:00', 6, '02:00:00', 'Weekly', NULL, NULL), -- Hamaden(HDVN)
  (1, 'E12', '10:30:00', 6, '02:00:00', 'Weekly', NULL, NULL), -- Sanwa VN
  
  (1, 'A01', '11:00:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
  (1, 'A02', '11:00:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
  (1, 'A03', '11:00:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- HAL
  
  (1, 'E15', '13:00:00', 6, '01:00:00', 'Weekly', NULL , NULL), -- Surteck
  (1, 'E19', '13:30:00', 6, '01:00:00', 'Weekly', NULL , NULL), -- Fertile
  
  (1, 'A01', '13:30:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
  (1, 'A02', '13:30:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
  (1, 'A03', '13:30:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- HAL
  
  (1, 'A01', '15:30:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
  (1, 'A02', '15:30:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
  (1, 'A03', '15:30:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- HAL
  
  (1, 'D04', '16:00:00', 6, '01:30:00', 'Weekly', NULL, NULL), -- Hamaden (HDVN)
  (1, 'E15', '16:00:00', 6, '01:30:00', 'Weekly', NULL, NULL), -- Surteck
  -- Nippo (pending..........)
  
  (1, 'A01', '16:30:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
  (1, 'A02', '16:30:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
  (1, 'A03', '16:30:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- HAL
  
  (1, 'A01', '19:00:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
  (1, 'A02', '19:00:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
  (1, 'A03', '19:00:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- HAL
  
  (1, 'A01', '19:30:00', 6, '01:00:00', 'Weekly', NULL, NULL), -- Ohara
  (1, 'A02', '19:30:00', 6, '01:00:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
  (1, 'A03', '19:30:00', 6, '01:00:00', 'Weekly', NULL, NULL), -- HAL
  (1, 'D04', '19:30:00', 6, '01:00:00', 'Weekly', NULL, NULL), -- Hamaden (HDVN)
  (1, 'E25', '19:30:00', 6, '01:00:00', 'Weekly', NULL, NULL), -- Kuroda
  
  (1, 'A01', '23:30:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
  (1, 'A02', '23:30:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
  (1, 'A03', '23:30:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- HAL
  
  (1, 'A01', '02:30:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
  (1, 'A02', '02:30:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
  (1, 'A03', '02:30:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- HAL
  
  (1, 'A01', '05:00:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- Ohara
  (1, 'A02', '05:00:00', 6, '00:30:00', 'Weekly', NULL, NULL), -- Matsuo --Ogino(pending..........)
  (1, 'A03', '05:00:00', 6, '00:30:00', 'Weekly', NULL, NULL) -- HAL
 ;

