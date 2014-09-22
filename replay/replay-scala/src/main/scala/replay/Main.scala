package replay
import scala.io.Source
import scala.util.matching.Regex._
import com.illposed.osc._
import java.net.InetAddress
import scala.collection.JavaConversions

object Main {
  //240.48332700 /muse/eeg/raw f 842.2299 f 832.36005 f 829.07007 f 861.9697
  val samplePattern = """(\d+\.\d+) /muse/eeg/raw(?: f (\d+\.\d+))(?: f (\d+\.\d+))(?: f (\d+\.\d+))(?: f (\d+\.\d+))""".r

  val sender = new OSCPortOut(InetAddress.getByName("localhost"), 3001)
  
  def main(args: Array[String]): Unit = {
    val samples = Source.fromFile("../chris.txt").getLines()
    val startTime = System.currentTimeMillis - getSampleTime(samples.drop(3).take(1).toSeq.head)
    for (line <- samples) {
      val eegData = (samplePattern findFirstMatchIn line) map eegSample
      eegData map {
        case (timestamp, a, b, c, d) => {
          val dueTime = startTime + timestamp
          while (System.currentTimeMillis < dueTime) {}
          val values = Seq[Object](a, b, c, d)
          val message = new OSCMessage("/muse/eeg/raw", JavaConversions.asJavaCollection(values))
          println(s"$timestamp - $a - $b - $c - $d")
          sender send message
        }
      }
    }
  }

  def eegSample(item: Match) = item match {
    case Groups(timestamp, a, b, c, d) => (
      (1000 * timestamp.toFloat).toLong,
      a.toFloat.asInstanceOf[Object], b.toFloat.asInstanceOf[Object], c.toFloat.asInstanceOf[Object], d.toFloat.asInstanceOf[Object]
    )
  }

  def getSampleTime(line: String) = eegSample((samplePattern findFirstMatchIn line).get)._1
}