<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class Image extends CI_Controller {
	public function thumbnail() {
		$file = $this->input->get('image', TRUE);

		$source_properties = getimagesize($file);
		$image_type = $source_properties[2];

		if ($image_type == IMAGETYPE_JPEG) {
			$image_resource_id = imagecreatefromjpeg($file);
		} elseif ($image_type == IMAGETYPE_GIF) {
			$image_resource_id = imagecreatefromgif($file);
		} elseif ($image_type == IMAGETYPE_PNG) {
			$image_resource_id = imagecreatefrompng($file);
		}

		$minimum = 300;

		$old_x = imageSX($image_resource_id);
  	$old_y = imageSY($image_resource_id);

		if ($old_x < $minimum || $old_y < $minimum) {
			if ($old_x < $old_y) {
				$ratio = ($minimum / $old_y);
			} else  {
				$ratio = ($minimum / $old_y);
			}

			$resizedWidth = $ratio * $old_x;
			$resizedHeight = $ratio * $old_y;
		}

		if ($old_x > $minimum || $old_y > $minimum) {
	     if ($old_x > $old_y) {
	          $ratio = ($minimum / $old_y);
	     } else {
	          $ratio = ($minimum / $old_x);
	     }

	     $resizedWidth = $ratio * $old_x;
	     $resizedHeight = $ratio * $old_y;
		}

		$target_width = $resizedWidth;
		$target_height = $resizedHeight;
		$target_layer = imagecreatetruecolor($target_width, $target_height);
		$target_layer_white = imagecolorallocate($target_layer, 255, 255, 255);

		imagefill($target_layer, 0, 0, $target_layer_white);
		imagecopyresampled($target_layer,$image_resource_id,0,0,0,0,$target_width,$target_height, $source_properties[0],$source_properties[1]);
		header('Content-Type: image/jpeg');
		imagejpeg($target_layer);
		imagedestroy($target_layer);
	}
}
